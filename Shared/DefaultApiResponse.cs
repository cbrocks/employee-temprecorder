using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Shared
{
    public static class DefaultApiResponse
    {
        public static string Create(object content, int statusCode, string message)
        {
            ApiResponse apiResponse;
            string result;

            result = JsonConvert.SerializeObject(content);
            apiResponse = new ApiResponse(result, statusCode, message);
            result = JsonConvert.SerializeObject(apiResponse);

            return result;
        }
    }
}
