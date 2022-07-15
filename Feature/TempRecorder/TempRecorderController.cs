using EmployeeTempRecorder.Feature.TempRecorder.Search;
using EmployeeTempRecorder.Infrastructure.Logging;
using EmployeeTempRecorder.Infrastructure.Mediator;
using EmployeeTempRecorder.Model;
using EmployeeTempRecorder.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder
{
    [Route("api/v1/temprecorder")]
    [ApiController]
    public class TempRecorderController : BaseController
    {
        private readonly IHttpContextAccessor _accessor;
        private HttpResponse _response => _accessor.HttpContext.Response;
        IHeaderDictionary _headers => _accessor.HttpContext.Request.Headers;
       

        public TempRecorderController(IMediator mediator, IHttpContextAccessor accessor) : base(mediator)
        {
            _accessor = accessor;
        }

        [HttpPost("addemployee")]
        public async Task AddEmployee([FromBody] JObject inputValue)
        {
            string responseBody;
            string inputstr = inputValue == null ? "" : inputValue.ToString();
            dynamic emp = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee>(inputstr);

            var response = _mediator.Send(new AddEmployeeCommand { Employee = emp });
            int statusCode = response.Data == null ? StatusCodes.Status500InternalServerError : response.Data.StatusCode;
            if (!response.HasException())
            {
                responseBody = DefaultApiResponse.Create(response.Data.IsSuccess, response.Data.StatusCode, response.Data.Message);
            }
            else
            {
                responseBody = DefaultApiResponse.Create(null, StatusCodes.Status500InternalServerError, response.Exception.Message);
            }
            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            var bytes = Encoding.UTF8.GetBytes(responseBody);
            _response.ContentLength = bytes.Length;

            await _response.Body.WriteAsync(bytes, 0, bytes.Length);
        }

        [HttpPost("updateemployee")]
        public async Task UpdateEmployee([FromBody] JObject inputValue)
        {
            string responseBody;
            string inputstr = inputValue == null ? "" : inputValue.ToString();
            dynamic emp = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee>(inputstr);

            var response = _mediator.Send(new UpdateEmployeeCommand { Employee = emp });
            int statusCode = response.Data == null ? StatusCodes.Status500InternalServerError : response.Data.StatusCode;
            if (!response.HasException())
            {
                responseBody = DefaultApiResponse.Create(response.Data.IsSuccess, response.Data.StatusCode, response.Data.Message);
            }
            else
            {
                responseBody = DefaultApiResponse.Create(null, StatusCodes.Status500InternalServerError, response.Exception.Message);
            }
            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            var bytes = Encoding.UTF8.GetBytes(responseBody);
            _response.ContentLength = bytes.Length;

            await _response.Body.WriteAsync(bytes, 0, bytes.Length);
        }

        [HttpGet("getallemployees")]
        public async Task GetAllEmployees()
        {
            byte[] bytes;

            var response = _mediator.Request(new AllEmployeeQuery { });
            string responseBody;
            int statusCode = response.Data == null ? StatusCodes.Status500InternalServerError : response.Data.StatusCode;
            if (!response.HasException())
            {
                responseBody = DefaultApiResponse.Create(response.Data.Employees, response.Data.StatusCode, response.Data.Message);
            }
            else
            {
                responseBody = DefaultApiResponse.Create(null, StatusCodes.Status500InternalServerError, response.Exception.Message);
            }
            _response.StatusCode = statusCode;
            bytes = Encoding.UTF8.GetBytes(responseBody);
            _response.ContentLength = bytes.Length;
            _response.ContentType = "application/json";

            await _response.Body.WriteAsync(bytes, 0, bytes.Length);
        }


        [HttpGet("search")]
        public async Task Search(string name, int? employeenumber=0, float? starttemp=0, float? endtemp=0, string startdate="", string enddate="")
        {
            byte[] bytes;

            var response = _mediator.Request(new SearchQuery
            {
                EmployeeNumber = employeenumber,
                EmployeeName = name,
                StartTemp = starttemp,
                EndTemp = endtemp,
                StartDate = startdate,
                EndDate = enddate
            });
            string responseBody;
            int statusCode = response.Data == null ? StatusCodes.Status500InternalServerError : response.Data.StatusCode;
            if (!response.HasException())
            {
                responseBody = DefaultApiResponse.Create(response.Data.Employees, response.Data.StatusCode, response.Data.Message);
            }
            else
            {
                responseBody = DefaultApiResponse.Create(null, StatusCodes.Status500InternalServerError, response.Exception.Message);
            }
            _response.StatusCode = statusCode;
            bytes = Encoding.UTF8.GetBytes(responseBody);
            _response.ContentLength = bytes.Length;
            _response.ContentType = "application/json";

            await _response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
