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
            Console.WriteLine("Please enter the licenses you would like checked.");
            ConvertDataToList dataToList = new ConvertDataToList();
            List<string> LicensesToCheck = dataToList.ConvertDataToStringList();

            // this code verifies the raw data has been turned into a list we can work with
            //foreach(string license in LicensesToCheck)
            //{
            //    Console.WriteLine(license);
            //}
            Console.Read();
        }
    }
}
