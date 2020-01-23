using LicenseStatusChecker_Common;
using OfficeOpenXml;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelFileReader reader = new ExcelFileReader(new Logger());
            ExcelFileWriter writer = new ExcelFileWriter();

            Console.WriteLine($"The program started at {CommonCode.Now}.");
            var licenseList = reader.ReadSpreadSheet(SharedFilePaths.readPath);

            var driver = new FirefoxDriver(SharedFilePaths.driverLocation);
            LicenseChecker checker = new LicenseChecker(driver, licenseList, new Logger());
            var tradesmenToSend = checker.InputLicenses();

            writer.WriteDataToFile(tradesmenToSend, SharedFilePaths.sendPath);

            Console.WriteLine("The check has been completed.");
            Console.WriteLine($"The program successfully completed at {CommonCode.Now}.");
            Console.Read();
        }
    }
}
