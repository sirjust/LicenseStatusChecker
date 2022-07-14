namespace LicenseStatusChecker_Common
{
    public static class SharedFilePaths
    {

        public static string readPath = $"..\\..\\..\\Files\\listToCheck.xlsx";
        public static string sendPath = $"..\\..\\..\\Files\\Send-{CommonCode.FormattedDate}.xlsx";
        public static string doNotSendPath = $"..\\..\\..\\Files\\DoNotSend-{CommonCode.FormattedDate}.xlsx";

        public static string expiredLog = $"..\\..\\..\\Files\\expired.txt";
        public static string exceptionLog = $"..\\..\\..\\Files\\exception.txt";
        public static string greaterThan90Log = $"..\\..\\..\\Files\\greaterThan90.txt";

        public static string coursesAlreadyTakenLog = $"..\\..\\..\\Files\\coursesAlreadyTaken.xlsx";

        public static string driverLocation = $"..\\..\\..\\packages\\Selenium.Firefox.WebDriver.0.24.0\\driver\\";
    }
}
