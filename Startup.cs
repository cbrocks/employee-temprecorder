using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Infrastructure.DependencyManager;
using EmployeeTempRecorder.Infrastructure.Mediator;
using EmployeeTempRecorder.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeTempRecorder
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connstring = Configuration.GetConnectionString("TempRecorderDb");

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddDistributedMemoryCache();
            services.AddSession(o => { o.Cookie.Name = ".TempRecorder.Session"; o.IdleTimeout = TimeSpan.FromSeconds(60); });
            services.AddOptions();
            services.AddControllers()
                .AddNewtonsoftJson();
            //services.AddEntityFrameworkSqlServer()
            services.AddDbContext<TempRecDbContext>(o => o.UseSqlServer(connstring));
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<SingleInstanceFactory>(sp => t => sp.GetService(t));
            services.AddTransient<MultiInstanceFactory>(sp => t => sp.GetServices(t));
            services.AddMediatorHandlers(typeof(Startup).GetTypeInfo().Assembly);

            var container = new Container(cfg =>
            {
                cfg.Scan(_ =>
                {
                    _.AssemblyContainingType<Startup>(); 
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    _.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    _.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    _.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                });
                cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                cfg.For<IMediator>().Use<Mediator>();
                cfg.For<IDependencyResolver>().Use<StructureMapDependencyResolver>();
                cfg.For<IHttpContextAccessor>().Use<HttpContextAccessor>().Singleton();

                cfg.Populate(services);
            });
            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
       public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // other code remove for clarity 
            loggerFactory.AddFile("Logs/Log-{Date}.txt");

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseSession();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{param1?}/{param2?}/{param3?}");
            });
        }
    }
}
