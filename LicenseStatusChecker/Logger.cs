using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public class Logger : ILogger
    {
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
