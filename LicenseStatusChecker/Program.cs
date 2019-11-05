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
            var now = DateTime.Now;
            List<List<Tradesman>> licenseList = new List<List<Tradesman>>();
            ExcelFileWriter writer = new ExcelFileWriter();
            ExcelFileReader reader = new ExcelFileReader();

            Console.WriteLine($"The program started at {now}.");
            licenseList = reader.readSpreadSheet(FilePaths.readPath);

            var driver = new FirefoxDriver(@"../../../packages/Selenium.Firefox.WebDriver.0.24.0/driver/");
            LicenseChecker checker = new LicenseChecker(driver, now, licenseList);
            var tradesmenToSend = checker.InputLicenses();

            writer.WriteDataToFile(tradesmenToSend, FilePaths.sendPath);

            Console.WriteLine("The check has been completed.");
            Console.WriteLine($"The program successfully completed at {DateTime.Now}.");
            Console.Read();
        }
    }
}
