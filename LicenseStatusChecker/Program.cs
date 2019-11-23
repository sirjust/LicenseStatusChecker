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
            ExcelFileReader reader = new ExcelFileReader(new Logger());
            ExcelFileWriter writer = new ExcelFileWriter();

            Console.WriteLine($"The program started at {now}.");
            var licenseList = reader.ReadSpreadSheet(FilePaths.readPath);

            var driver = new FirefoxDriver(FilePaths.driverLocation);
            LicenseChecker checker = new LicenseChecker(driver, now, licenseList, new Logger());
            var tradesmenToSend = checker.InputLicenses();

            writer.WriteDataToFile(tradesmenToSend, FilePaths.sendPath);

            Console.WriteLine("The check has been completed.");
            Console.WriteLine($"The program successfully completed at {DateTime.Now}.");
            Console.Read();
        }
    }
}
