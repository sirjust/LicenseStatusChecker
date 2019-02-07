using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Please enter the licenses you would like checked.");
            //ConvertDataToList dataToList = new ConvertDataToList();
            //List<string> LicensesToCheck = dataToList.ConvertDataToStringList();

            //CheckLicenses checkLicenses = new CheckLicenses();
            //checkLicenses.inputLicenses(LicensesToCheck);

            //Console.WriteLine("The check has been completed.");
            //Console.Read();
            List<Tradesman> licenseList = new List<Tradesman>();
            WriteToExcelFile write = new WriteToExcelFile();
            readExcelFile read = new readExcelFile();
            read.readSpreadSheet(FilePaths.readPath);
            // write.WriteDataToFile();

            Console.Read();


            // this code verifies the raw data has been turned into a list we can work with
            // we can use it later to verify all data has been mapped
            //foreach(string license in LicensesToCheck)
            //{
            //    Console.WriteLine(license);
            //}
        }
    }
}
