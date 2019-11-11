using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public static class FilePaths
    {
        private static string formattedDate = $"{DateTime.Today.Month.ToString()}-{DateTime.Today.Day.ToString()}-{DateTime.Today.Year.ToString()}";
        public static string readPath = $"..\\..\\..\\Files\\listToCheck.xlsx";
        public static string sendPath = $"..\\..\\..\\Files\\Send-{formattedDate}.xlsx";
        public static string doNotSendPath = $"..\\..\\..\\Files\\DoNotSend-{formattedDate}.xlsx";

        public static string expiredLog = $"..\\..\\..\\Files\\expired.txt";
        public static string exceptionLog = $"..\\..\\..\\Files\\exception.txt";
        public static string greaterThan90Log = $"..\\..\\..\\Files\\greaterThan90.txt";

        public static string driverLocation = $"..\\..\\..\\packages\\Selenium.Firefox.WebDriver.0.24.0\\driver\\";
    }
}
