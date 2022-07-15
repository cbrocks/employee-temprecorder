using EmployeeTempRecorder.Infrastructure.CQRS;
using EmployeeTempRecorder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder
{
    public class AddEmployeeCommand : ICommand<EmployeeResponse>
    {
        public Employee Employee { get; set; }
    }
}
