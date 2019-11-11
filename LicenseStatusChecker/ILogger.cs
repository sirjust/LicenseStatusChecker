using System.IO;

namespace LicenseStatusChecker
{
    public interface ILogger
    {
        void WriteErrorsToLog(string logMessage, string path);

        void WriteToConsole(string message);
    }
}