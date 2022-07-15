using EmployeeTempRecorder.Model;
using System;
using System.Text;

namespace EmployeeTempRecorder.Infrastructure.Logging
{
    public class LogRepository : ILogRepository
    {
        private readonly TempRecDbContext _core;

        public LogRepository(TempRecDbContext core)
        {
            _core = core;
        }
     
        public void Audit(string category, string source, string message, Severity level)
        {
            DateTime now = DateTime.Now;

            var entry = new AuditEntry
            {
                CreateDate = now,
                Category = category,
                Source = source,
                Message = message,
                Severity = (int)level,
            };
            _core.AuditEntries.Add(entry);
            _core.SaveChanges();
        }
    }
}
