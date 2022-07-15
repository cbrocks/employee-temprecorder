using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Infrastructure.Logging
{
    public enum Severity
    {
        Information = 0,
        Audit = 1,
        Warning = 2,
        Error = 3,
        SevereError = 4,
        Fatal = 5
    }
}
