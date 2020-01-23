using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public class Logger : ILogger
    {
        public void LogStart()
        {
            Console.WriteLine($"The program started at {CommonCode.Now}.");
        }

        public void LogEnd()
        {
            Console.WriteLine("The check has been completed.");
            Console.WriteLine($"The program successfully completed at {CommonCode.Now}.");
            Console.Read();
        }
        public void WriteErrorsToLog(string logMessage, string path)
        {
            var sw = new StreamWriter(path, true);
            sw.Write("\r\nLog Entry : ");
            sw.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            sw.WriteLine(logMessage);
            sw.WriteLine("-------------------------------");
            sw.Close();
        }

        public void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }
    }
}
