using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder.GetAllEmployee
{
    public class AllEmployeeHandler : IRequestHandler<AllEmployeeQuery, AllEmployeeResponse>
    {
        private TempRecDbContext tempRecDbContext;
        private readonly ILogger _logger;
        public AllEmployeeHandler(TempRecDbContext _tempRecDbContext, ILogger<AddEmployeeHandler> logger)
        {
            tempRecDbContext = _tempRecDbContext;
            _logger = logger;
        }
        public AllEmployeeResponse Handle(AllEmployeeQuery request)
        {
            var response = new AllEmployeeResponse();

            try
            {
                var employees = tempRecDbContext.Employees.ToList();

                if (employees.Any())
                {
                    response.Employees = employees;
                    response.StatusCode = 200;
                    response.Message = $"Returning {employees.Count} employee(s)";
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No record found!";
                }
            }
            catch (Exception ex)
            {
                response.Employees = null;
                response.StatusCode = 500;
                response.Message = ex.Message;
                _logger.LogInformation("An error has occured while retrieving employee records.\n See details: {0}", ex.StackTrace.ToString());
            }
            return response;
        }
    }
}
