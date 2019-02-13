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
            List<Tradesman> licenseList = new List<Tradesman>();
            WriteToExcelFile write = new WriteToExcelFile();
            readExcelFile read = new readExcelFile();
            licenseList = read.readSpreadSheet(FilePaths.readPath);

            CheckLicenses checkLicenses = new CheckLicenses();
            List<Tradesman> tradesmenToSend = checkLicenses.inputLicenses(licenseList);

            write.WriteDataToFile(tradesmenToSend, FilePaths.sendPath);

            Console.WriteLine("The check has been completed.");
            Console.Read();
        }
    }
}
