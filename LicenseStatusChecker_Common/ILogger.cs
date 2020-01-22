using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public interface ILogger
    {
        void WriteErrorsToLog(string logMessage, string path);

        void WriteToConsole(string message);
    }
}
