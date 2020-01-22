using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker.Tests
{
    public static class TestHelper
    {
        public static List<WashingtonTradesman> GetMockTradesmen()
        {
            var tradesmen = new List<WashingtonTradesman>() {
                new WashingtonTradesman{ Name= "JONES, A", ExpirationDate = DateTime.Now.AddDays(30).ToString(), HoursCompleted = 0, LicenseType = "PL" }
            };
            return tradesmen;
        }

        public static WashingtonTradesman GetMockTradesman()
        {
            return new WashingtonTradesman { Name = "JONES, A", ExpirationDate = DateTime.Now.AddDays(30).ToString(), HoursCompleted = 0, LicenseType = "PL" };
        }
    }
}
