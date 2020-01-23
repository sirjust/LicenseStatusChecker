using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;

namespace LicenseStatusChecker
{
    public class LicenseChecker
    {
        IWebDriver _driver;
        List<List<ITradesman>> _tradesmen;
        WebDriverWait _wait;
        ILogger _logger;
        ExcelFileWriter _writer;

        public LicenseChecker(IWebDriver driver, List<List<ITradesman>> tradesmen, ILogger logger, ExcelFileWriter writer)
        {
            _driver = driver;
            _tradesmen = tradesmen;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _logger = logger;
            _writer = writer;
        }

        public List<ITradesman> InputLicenses()
        {
            // Console.WriteLine(thisPath);
            List<ITradesman> tradesmenToSend = new List<ITradesman>();
            List<ITradesman> doNotSend = new List<ITradesman>();

            foreach (List<ITradesman> tradeList in _tradesmen)
            {
                foreach (ITradesman WashingtonTradesman in tradeList)
                {
                    try
                    {
                        _driver.Url = $"https://secure.lni.wa.gov/verify/Detail.aspx?UBI=&LIC={WashingtonTradesman.LicenseNumber}&SAW=";

                        var expirationInfo = CheckExpirationDate();
                        if (expirationInfo.Item1 > 90) // if the expiration date is far in the future, the WashingtonTradesman has already renewed
                        {
                            Console.WriteLine($"{WashingtonTradesman.LicenseNumber} has likely already renewed.");
                            WashingtonTradesman.NotSendReason = "Already renewed";
                            WashingtonTradesman.ExpirationDate = expirationInfo.Item2.ToString();
                            doNotSend.Add(WashingtonTradesman);
                            continue;
                        }

                        var status = GetLicenseStatus();
                        if (status != "Active")
                        {
                            // if the license is expired or inactive, we move on
                            Console.WriteLine($"{WashingtonTradesman.LicenseNumber} is not active.");
                            WashingtonTradesman.NotSendReason = "Not active";
                            doNotSend.Add(WashingtonTradesman);
                            continue;
                        }

                        SetTradeAndHours(WashingtonTradesman);

                        // now that all our properties are populated, we need to check them against the credits the WashingtonTradesman has already completed
                        bool hasCourses;
                        try
                        {
                            IWebElement coursesElement = _wait.Until(d => d.FindElement(By.Id("Courses")));
                            hasCourses = true;
                        }
                        catch
                        {
                            hasCourses = false;
                        }
                        if (!hasCourses)
                        {
                            // add license to the send list
                            continue;
                        }
                        var allDataItems = _driver.FindElements(By.XPath("//span[contains(text(),'.00')]"));
                        List<string> coursesTaken = new List<string>();
                        double numberOfCredits = 0.0;
                        foreach (var item in allDataItems)
                        {
                            string data = item.GetAttribute("innerHTML");
                            // if the dataItem represents a fine, we don't want it counted as credits
                            if (data.Contains("$"))
                            {
                                continue;
                            }
                            int index = data.IndexOf(" ");
                            if (index > 0)
                                data = data.Substring(0, index);
                            coursesTaken.Add(data);
                            numberOfCredits += Convert.ToDouble(data);
                        }
                        WashingtonTradesman.HoursCompleted = numberOfCredits;
                        Console.Write("{0} has completed {1} credits and needs {2}", WashingtonTradesman.LicenseNumber, WashingtonTradesman.HoursCompleted, WashingtonTradesman.HoursNeeded);
                        if (WashingtonTradesman.HoursNeeded <= numberOfCredits)
                        {
                            // if they have enough credits
                            Console.WriteLine(" | {0} has enough credits.", WashingtonTradesman.LicenseNumber);
                            WashingtonTradesman.NotSendReason = "CEUs completed";
                            doNotSend.Add(WashingtonTradesman);
                            continue;
                        }
                        Console.Write("\n");
                        tradesmenToSend.Add(WashingtonTradesman);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was a {0} with {1}.", ex.Message, WashingtonTradesman.LicenseNumber);
                        _logger.WriteErrorsToLog($"{ex}\n" + "There was a Selenium error.", SharedFilePaths.exceptionLog);
                        continue;
                    }
                }
                // here we document who will not receive a postcard and the reason why
                // i chose to do this here because i cannot return two values from the function
                _writer.WriteDataToFile(doNotSend, SharedFilePaths.doNotSendPath);
            }
            return tradesmenToSend;
        }

        private void SetTradeAndHours(ITradesman WashingtonTradesman)
        {
            // we need to check the trade and rank of the license holder
            IWebElement licenseTypeElement = _wait.Until(d => d.FindElement(By.Id("LicenseType")));
            WashingtonTradesman.Trade = licenseTypeElement.GetAttribute("innerHTML");
            WashingtonTradesman.HoursNeeded = WashingtonTradesman.GetHoursNeeded(WashingtonTradesman.Trade);
        }

        private string GetLicenseStatus()
        {
            IWebElement licenseStatus = _wait.Until(d => d.FindElement(By.XPath("//*[@id='StatusDescription']/strong")));
            return licenseStatus.GetAttribute("innerHTML");
        }

        private (int, DateTime) CheckExpirationDate()
        {
            //now we check the license's expiration date
            IWebElement expirationDateElement = _wait.Until<IWebElement>(d => d.FindElement(By.Id("ExpirationDate")));
            // this delay is here because there was an exception while parsing on the next line if it ran too quickly
            Task.Delay(1000).Wait();
            var expirationDate = DateTime.Parse(expirationDateElement.GetAttribute("innerHTML"));
            int daysTillExpiration = expirationDate.Subtract(CommonCode.Now).Days;
            var returnInfo = (daysTillExpiration, expirationDate);
            return returnInfo;
        }
    }
}
