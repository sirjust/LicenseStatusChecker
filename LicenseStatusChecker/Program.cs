using OfficeOpenXml;
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
            Console.WriteLine("The program started at {0}.", DateTime.Now);
            List<List<Tradesman>> licenseList = new List<List<Tradesman>>();
            WriteToExcelFile write = new WriteToExcelFile();
            readExcelFile read = new readExcelFile();
            licenseList = read.readSpreadSheet(FilePaths.readPath);
            CheckLicenses checkLicenses = new CheckLicenses();
            List<Tradesman> tradesmenToSend = new List<Tradesman>();
            tradesmenToSend = checkLicenses.inputLicenses(licenseList);

            write.WriteDataToFile(tradesmenToSend, FilePaths.sendPath);

            Console.WriteLine("The check has been completed.");
            Console.WriteLine("The program successfully completed at {0}.", DateTime.Now);
            Console.Read();
        }
    }
}
