using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Model
{
    [Table("AuditEntry")]
    public class AuditEntry 
    {
        [Key]
        public int AuditEntryId { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public int Severity { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
