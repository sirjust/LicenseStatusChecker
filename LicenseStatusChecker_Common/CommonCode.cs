using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public static class CommonCode
    {
        public readonly static DateTime Now = DateTime.Now;
        public readonly static string FormattedDate = $"{Now.Month.ToString()}-{Now.Day.ToString()}-{Now.Year.ToString()}";
    }
}
