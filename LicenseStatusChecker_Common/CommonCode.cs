using System;

namespace LicenseStatusChecker_Common
{
    public static class CommonCode
    {
        public static DateTime Now { get { return GetCurrentDateTime(); } set { GetCurrentDateTime(); } }
        public readonly static string FormattedDate = $"{Now.Month}-{Now.Day}-{Now.Year}";
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
