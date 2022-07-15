using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder.Search
{
    public class SearchHandler : IRequestHandler<SearchQuery, AllEmployeeResponse>
    {
        private TempRecDbContext tempRecDbContext;
        private readonly ILogger _logger;
        public SearchHandler(TempRecDbContext _tempRecDbContext, ILogger<AddEmployeeHandler> logger)
        {
            tempRecDbContext = _tempRecDbContext;
            _logger = logger;
        }
        public AllEmployeeResponse Handle(SearchQuery request)
        {
            var response = new AllEmployeeResponse();

            try
            {
                var employees = new List<Employee>();
                //apply search per field only
                if (!string.IsNullOrEmpty(request.EmployeeName))
                {
                    employees = tempRecDbContext.Employees.Where(e => e.FirstName.Contains(request.EmployeeName) || e.LastName.Contains(request.EmployeeName))
                        .OrderBy(o=>o.LastName)
                        .ToList();
                }
                else if (request.EmployeeNumber > 0)
                {
                    employees = tempRecDbContext.Employees.Where(e => e.EmployeeNumber == request.EmployeeNumber).ToList();
                }
                else if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    if (IsValidDate(request.StartDate) && IsValidDate(request.EndDate))
                    {
                        var startdate = Convert.ToDateTime(request.StartDate);
                        var enddate = Convert.ToDateTime(request.EndDate);
                        employees = tempRecDbContext.Employees.Where(e => e.RecordDate >= startdate && e.RecordDate <= enddate)
                            .OrderBy(o=>o.RecordDate)
                            .ToList();
                    }
                }
                else if (request.StartTemp > 0 && request.EndTemp > 0)
                {
                    employees = tempRecDbContext.Employees.Where(e => e.Temperature >= request.StartTemp && e.Temperature <= request.EndTemp)
                         .OrderBy(o => o.Temperature)
                        .ToList();
                }

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

        public bool IsValidDate(string dateTime)
        {
            string[] formats = { "yyyy-MM-dd" };
            DateTime parsedDateTime;
            return DateTime.TryParseExact(dateTime, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDateTime);
        }
    }
}
