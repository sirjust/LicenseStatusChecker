using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;
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
            Logger logger = new Logger();
            ExcelFileReader reader = new ExcelFileReader(logger);
            ExcelFileWriter writer = new ExcelFileWriter();

            logger.LogStart();
            var licenseList = reader.ReadSpreadSheet(SharedFilePaths.readPath, "WA");

            var driver = new FirefoxDriver(SharedFilePaths.driverLocation);
            LicenseChecker checker = new LicenseChecker(driver, licenseList, logger, writer);
            var tradesmenToSend = checker.InputLicenses();

            writer.WriteDataToFile(tradesmenToSend, SharedFilePaths.sendPath);

            logger.LogEnd();
        }
    }
}
