using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            List<ITradesman> tradesmenToSend = new List<ITradesman>();
            List<ITradesman> doNotSend = new List<ITradesman>();

            foreach (List<ITradesman> tradeList in _tradesmen)
            {
                foreach (ITradesman washingtonTradesman in tradeList)
                {
                    try
                    {
                        _driver.Url = $"https://secure.lni.wa.gov/verify/Detail.aspx?UBI=&LIC={washingtonTradesman.LicenseNumber}&SAW=";

                        if (CheckExpirationDate().Item1 > 90) // if the expiration date is far in the future, the WashingtonTradesman has already renewed
                        {
                            washingtonTradesman.ExpirationDate = CheckExpirationDate().Item2.ToString();
                            var reason = $"{washingtonTradesman.LicenseNumber} has likely already renewed.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            continue;
                        }

                        if (GetLicenseStatus() != "Active")
                        {
                            // if the license is expired or inactive, we move on
                            var reason = $"{washingtonTradesman.LicenseNumber} is not active.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            continue;
                        }

                        SetTradeAndHours(washingtonTradesman);

                        // now that all our properties are populated, we need to check them against the credits the WashingtonTradesman has already completed                        
                        if (!CheckCourses())
                        {
                            // add license to the send list
                            // I think there is a problem here
                            // This wasn't in here but I think it needs to be
                            tradesmenToSend.Add(washingtonTradesman);
                            continue;
                        }
                        var potentialCreditsElement = _driver.FindElements(By.XPath("//span[contains(text(),'.00')]"));
                        List<string> coursesTaken = new List<string>();
                        double numberOfCredits = 0.0;
                        foreach (var element in potentialCreditsElement)
                        {
                            string elementText = element.GetAttribute("innerHTML");
                            // if the dataItem represents a fine, we don't want it counted as credits
                            if (elementText.Contains("$"))
                            {
                                continue;
                            }
                            int index = elementText.IndexOf(" ");
                            if (index > 0)
                                elementText = elementText.Substring(0, index);
                            coursesTaken.Add(elementText);
                            numberOfCredits += Convert.ToDouble(elementText);
                        }
                        washingtonTradesman.HoursCompleted = numberOfCredits;
                        Console.WriteLine($"{washingtonTradesman.LicenseNumber} has completed {washingtonTradesman.HoursCompleted} credits and needs {washingtonTradesman.HoursNeeded}");
                        if (washingtonTradesman.HoursNeeded <= numberOfCredits)
                        {
                            var reason = $" | {washingtonTradesman.LicenseNumber} has enough credits.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            continue;
                        }
                        tradesmenToSend.Add(washingtonTradesman);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"There was a {ex.Message} with {washingtonTradesman.LicenseNumber}.");
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

        public void AddTradesmanToDoNotSendList(List<ITradesman> list, ITradesman tradesman, string reason)
        {
            Console.WriteLine(reason);
            tradesman.NotSendReason = "CEUs completed";
            list.Add(tradesman);
        }

        public bool CheckCourses()
        {
            try
            {
                IWebElement coursesElement = _wait.Until(d => d.FindElement(By.Id("Courses")));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetTradeAndHours(ITradesman WashingtonTradesman)
        {
            // we need to check the trade and rank of the license holder
            IWebElement licenseTypeElement = _wait.Until(d => d.FindElement(By.Id("LicenseType")));
            WashingtonTradesman.Trade = licenseTypeElement.GetAttribute("innerHTML");
            WashingtonTradesman.HoursNeeded = WashingtonTradesman.GetHoursNeeded(WashingtonTradesman.Trade);
        }

        public string GetLicenseStatus()
        {
            IWebElement licenseStatus = _wait.Until(d => d.FindElement(By.XPath("//*[@id='StatusDescription']/strong")));
            return licenseStatus.GetAttribute("innerHTML");
        }

        public (int, DateTime) CheckExpirationDate()
        {
            //now we check the license's expiration date
            IWebElement expirationDateElement = _wait.Until(d => d.FindElement(By.Id("ExpirationDate")));
            // this delay is here because there was an exception while parsing on the next line if it ran too quickly
            Task.Delay(500).Wait();
            var expirationDate = DateTime.Parse(expirationDateElement.GetAttribute("innerHTML"));
            int daysTillExpiration = expirationDate.Subtract(CommonCode.Now).Days;
            var returnInfo = (daysTillExpiration, expirationDate);
            return returnInfo;
        }
    }
}
