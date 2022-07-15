using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder.UpdateEmployee
{
    public class UpdateEmployeeHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeResponse>
    {
        private TempRecDbContext tempRecDbContext;
        private readonly ILogger _logger;

        public UpdateEmployeeHandler(TempRecDbContext _tempRecDbContext, ILogger<AddEmployeeHandler> logger)
        {
            tempRecDbContext = _tempRecDbContext;
            _logger = logger;
        }

        public EmployeeResponse Handle(UpdateEmployeeCommand command)
        {
            var response = new EmployeeResponse();
            using (var transaction = tempRecDbContext.Database.BeginTransaction())
            {
                try
                {
                    var emp = tempRecDbContext.Employees.SingleOrDefault(e => e.EmployeeNumber == command.Employee.EmployeeNumber);
                    if(emp!=null)
                    {
                        emp.FirstName = command.Employee.FirstName;
                        emp.LastName = command.Employee.FirstName;
                        emp.Temperature = command.Employee.Temperature;
                        emp.RecordDate = command.Employee.RecordDate;
                        tempRecDbContext.Employees.Update(emp);

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
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Unable to update employee record.";
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        return response;
                    }
                   
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                    response.Message = ex.StackTrace.ToString();
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    _logger.LogInformation("An error has occured while updating an employee.\n See details: {0}", ex.StackTrace.ToString());
                }
            }
            return response;
        }
    }
}
