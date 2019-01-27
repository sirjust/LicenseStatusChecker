using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    class ConvertDataToList
    {
        public List<string> ConvertDataToStringList()
        {
            string s = "1";
            List<string> result = new List<string> { };
            while (!string.IsNullOrEmpty(s))
            {
                s = Console.ReadLine();
                result.Add(s);
            }
            return result;
        }
    }
}
