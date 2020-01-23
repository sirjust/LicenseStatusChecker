using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public static class CommonCode
    {
        public static DateTime Now { get { return GetCurrentDateTime(); } set { GetCurrentDateTime(); } }
        public readonly static string FormattedDate = $"{Now.Month.ToString()}-{Now.Day.ToString()}-{Now.Year.ToString()}";
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
