using System;

namespace EmployeeTempRecorder.Infrastructure.Logging {

    public interface ILogRepository {

        void Audit(string category, string source, string message, Severity level);
    }
}
