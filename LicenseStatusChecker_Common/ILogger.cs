namespace LicenseStatusChecker_Common
{
    public interface ILogger
    {
        void LogEnd();
        void LogStart();
        void WriteErrorsToLog(string logMessage, string path);

        void WriteToConsole(string message);
    }
}
