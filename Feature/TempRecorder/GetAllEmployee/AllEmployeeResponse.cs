
using EmployeeTempRecorder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder
{
    public class AllEmployeeResponse
    {
        public List<Employee> Employees { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
