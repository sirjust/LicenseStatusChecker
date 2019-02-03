using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public class RecordLicensesToSend
    {
        public void record(string logMessage, TextWriter sw)
        {
            sw.WriteLine("Send to: {0}", logMessage);
        }
    }
}
