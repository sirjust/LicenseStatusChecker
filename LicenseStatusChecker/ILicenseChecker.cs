using LicenseStatusChecker_Common;
using System;
using System.Collections.Generic;

namespace LicenseStatusChecker
{
    public interface ILicenseChecker
    {
        void AddTradesmanToDoNotSendList(List<ITradesman> list, ITradesman tradesman, string reason);
        bool CheckCourses();
        (int, DateTime) CheckExpirationDate();
        string GetLicenseStatus();
        void CheckLicenses(List<List<ITradesman>> tradesmen);
        void SetTradeAndHours(ITradesman WashingtonTradesman);
    }
}