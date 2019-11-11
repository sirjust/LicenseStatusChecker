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

namespace LicenseStatusChecker
{
    public class LicenseChecker
    {
        IWebDriver _driver;
        DateTime _todaysDate = DateTime.Today;
        List<List<Tradesman>> _tradesmen;
        WebDriverWait _wait;
        ILogger _logger;

        public LicenseChecker(IWebDriver driver, DateTime today, List<List<Tradesman>> tradesmen, ILogger logger)
        {
            _driver = driver;
            _todaysDate = today;
            _tradesmen = tradesmen;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _logger = logger;
        }

        public List<Tradesman> InputLicenses()
        {
            // Console.WriteLine(thisPath);
            List<Tradesman> tradesmenToSend = new List<Tradesman>();
            List<Tradesman> doNotSend = new List<Tradesman>();

            foreach (List<Tradesman> tradeList in _tradesmen)
            {
                foreach (Tradesman tradesman in tradeList)
                {
                    try
                    {
                        _driver.Url = $"https://secure.lni.wa.gov/verify/Detail.aspx?UBI=&LIC={tradesman.LicenseNumber}&SAW=";

                        var expirationInfo = CheckExpirationDate();
                        if (expirationInfo.Item1 > 90) // if the expiration date is far in the future, the tradesman has already renewed
                        {
                            Console.WriteLine("{0} has likely already renewed.", tradesman.LicenseNumber);
                            tradesman.NotSendReason = "Already renewed";
                            tradesman.ExpirationDate = expirationInfo.Item2.ToString();
                            doNotSend.Add(tradesman);
                            continue;
                        }

                        var status = GetLicenseStatus();
                        if (status != "Active")
                        {
                            // if the license is expired or inactive, we move on
                            Console.WriteLine("{0} is not active.", tradesman.LicenseNumber);
                            tradesman.NotSendReason = "Not active";
                            doNotSend.Add(tradesman);
                            continue;
                        }

                        SetTradeAndHours(tradesman);

                        // now that all our properties are populated, we need to check them against the credits the tradesman has already completed
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
                        tradesman.HoursCompleted = numberOfCredits;
                        Console.Write("{0} has completed {1} credits and needs {2}", tradesman.LicenseNumber, tradesman.HoursCompleted, tradesman.HoursNeeded);
                        if (tradesman.HoursNeeded <= numberOfCredits)
                        {
                            // if they have enough credits
                            Console.WriteLine(" | {0} has enough credits.", tradesman.LicenseNumber);
                            tradesman.NotSendReason = "CEUs completed";
                            doNotSend.Add(tradesman);
                            continue;
                        }
                        Console.Write("\n");
                        tradesmenToSend.Add(tradesman);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was a {0} with {1}.", ex.Message, tradesman.LicenseNumber);
                        _logger.WriteErrorsToLog($"{ex}\n" + "There was a Selenium error.", FilePaths.exceptionLog);
                        continue;
                    }
                    // here we document who will not receive a postcard and the reason why
                    // i chose to do this here because i cannot return two values from the function
                    ExcelFileWriter write = new ExcelFileWriter();
                    write.WriteDataToFile(doNotSend, FilePaths.doNotSendPath);
                }
            }
            return tradesmenToSend;
        }

        private void SetTradeAndHours(Tradesman tradesman)
        {
            // we need to check the trade and rank of the license holder
            IWebElement licenseTypeElement = _wait.Until(d => d.FindElement(By.Id("LicenseType")));
            tradesman.Trade = licenseTypeElement.GetAttribute("innerHTML");
            tradesman.HoursNeeded = tradesman.GetHoursNeeded(tradesman.Trade);
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
            int daysTillExpiration = expirationDate.Subtract(_todaysDate).Days;
            var returnInfo = (daysTillExpiration, expirationDate);
            return returnInfo;
        }
    }
}
