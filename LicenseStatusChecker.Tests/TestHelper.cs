using System;
using System.Collections.Generic;
using System.Text;
using LicenseStatusChecker;

namespace LicenseStatusChecker.Tests
{
    public static class TestHelper
    {
        public static List<Tradesman> GetMockTradesmen()
        {
            var tradesmen = new List<Tradesman>() {
                new Tradesman{ Name= "JONES, A", ExpirationDate = DateTime.Now.AddDays(30).ToString(), HoursCompleted = 0, LicenseType = "PL" }
            };
            return tradesmen;
        }

        public static Tradesman GetMockTradesman()
        {
            return new Tradesman { Name = "JONES, A", ExpirationDate = DateTime.Now.AddDays(30).ToString(), HoursCompleted = 0, LicenseType = "PL" };
        }
    }
}
