using EmployeeTempRecorder.Infrastructure.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Feature.TempRecorder.Search
{
    public class SearchQuery : IRequest<AllEmployeeResponse>
    {
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public float? StartTemp { get; set; }
        public float? EndTemp { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
