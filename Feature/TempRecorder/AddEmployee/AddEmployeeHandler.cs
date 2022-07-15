using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder
{
    public class AddEmployeeHandler : ICommandHandler<AddEmployeeCommand, EmployeeResponse>
    {
        private TempRecDbContext tempRecDbContext;
        private readonly ILogger _logger;

        public AddEmployeeHandler(TempRecDbContext _tempRecDbContext, ILogger<AddEmployeeHandler> logger)
        {
            tempRecDbContext = _tempRecDbContext;
            _logger = logger;
        }

        public EmployeeResponse Handle(AddEmployeeCommand command)
        {
            var response = new EmployeeResponse();
            using (var transaction = tempRecDbContext.Database.BeginTransaction())
            {
                try
                {
                    tempRecDbContext.Employees.Add(command.Employee);
                    if (tempRecDbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Success";
                        response.StatusCode = StatusCodes.Status201Created;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed";
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                    response.Message = ex.StackTrace.ToString();
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    _logger.LogInformation("An error has occured while adding an employee.\n See details: {0}", ex.StackTrace.ToString());
                }
            }
            return response;
        }
    }
}
