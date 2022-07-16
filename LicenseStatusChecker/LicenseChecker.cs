using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public class LicenseChecker : ILicenseChecker
    {
        IWebDriver _driver;
        WebDriverWait _wait;
        ILogger _logger;
        IWriter _writer;

        public LicenseChecker(IWebDriver driver, ILogger logger, IWriter writer)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _logger = logger;
            _writer = writer;
        }

        public void CheckLicenses(List<List<ITradesman>> tradesmen)
        {
            var tradesmenToSend = new List<ITradesman>();
            var doNotSend = new List<ITradesman>();
            var coursesAlreadyTaken = new Dictionary<string, int>();

            foreach (List<ITradesman> tradeList in tradesmen)
            {
                foreach (ITradesman washingtonTradesman in tradeList)
                {
                    try
                    {
                        _driver.Url = $"https://secure.lni.wa.gov/verify/Detail.aspx?UBI=&LIC={washingtonTradesman.LicenseNumber}&SAW=";
                        var expiration = CheckExpirationDate();

                        if (expiration.Item1 > 90) // if the expiration date is far in the future, the WashingtonTradesman has already renewed
                        {
                            washingtonTradesman.ExpirationDate = expiration.Item2.ToString();
                            var reason = $"{washingtonTradesman.LicenseNumber} has likely already renewed.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            _writer.WriteSingleTradesmanToFile(washingtonTradesman, SharedFilePaths.doNotSendPath);
                            continue;
                        }

                        if (expiration.Item1 == -1)
                        {
                            var reason = $"{washingtonTradesman.LicenseNumber} is not a valid license.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            continue;
                        }

                        if (GetLicenseStatus() != "Active")
                        {
                            // if the license is expired or inactive, we move on
                            var reason = $"{washingtonTradesman.LicenseNumber} is not active.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            _writer.WriteSingleTradesmanToFile(washingtonTradesman, SharedFilePaths.doNotSendPath);
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

                        if (numberOfCredits > 0)
                        {
                            // find course names
                            var singleCols = _driver.FindElement(By.Id("coursesList")).FindElements(By.ClassName("itemSingleCol"));
                            foreach(var element in singleCols)
                            {
                                try
                                {
                                    var nameLabel = element.FindElement(By.CssSelector("label"));

                                    var sibling = element.FindElement(By.XPath("following-sibling::div")).FindElement(By.XPath("following-sibling::div"));
                                    var codeLabel = sibling.FindElement(By.CssSelector("label"));
                                    
                                    if (nameLabel.GetAttribute("innerHTML") == "Course title" && codeLabel.GetAttribute("innerHTML") == "Course code")
                                    {
                                        var course = nameLabel.FindElement(By.XPath("following-sibling::span"));
                                        var courseName = course.GetAttribute("innerHTML");

                                        var codeElement = codeLabel.FindElement(By.XPath("following-sibling::span"));
                                        var code = codeElement.GetAttribute("innerHTML");

                                        var namePlusCode = $"{courseName}`{code}";

                                        if (coursesAlreadyTaken.ContainsKey(namePlusCode))
                                        {
                                            coursesAlreadyTaken[namePlusCode]++;
                                        }
                                        else
                                        {
                                            coursesAlreadyTaken.Add(namePlusCode, 1);
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Couldn't determine course used. If this persists contact support.");
                                    continue;
                                }
                            }
                        }

                        if (washingtonTradesman.HoursNeeded <= numberOfCredits)
                        {
                            var reason = $" | {washingtonTradesman.LicenseNumber} has enough credits.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            _writer.WriteSingleTradesmanToFile(washingtonTradesman, SharedFilePaths.doNotSendPath);
                            continue;
                        }
                        if (numberOfCredits > 0)
                        {
                            var reason = $" | {washingtonTradesman.LicenseNumber} has some credits already.";
                            AddTradesmanToDoNotSendList(doNotSend, washingtonTradesman, reason);
                            _writer.WriteSingleTradesmanToFile(washingtonTradesman, SharedFilePaths.doNotSendPath);
                            continue;
                        }
                        tradesmenToSend.Add(washingtonTradesman);
                        _writer.WriteSingleTradesmanToFile(washingtonTradesman, SharedFilePaths.sendPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"There was a {ex.Message} with {washingtonTradesman.LicenseNumber}.");
                        _logger.WriteErrorsToLog($"{ex}\n" + "There was a Selenium error.", SharedFilePaths.exceptionLog);
                        continue;
                    }
                }
                //_writer.WriteDataToFile(doNotSend, SharedFilePaths.doNotSendPath);
            }
            _writer.WriteAlreadyTakenCourses(coursesAlreadyTaken);
        }

        public void AddTradesmanToDoNotSendList(List<ITradesman> list, ITradesman tradesman, string reason)
        {
            Console.WriteLine(reason);
            tradesman.NotSendReason = reason;
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

            if(_driver.Url == "https://secure.lni.wa.gov/verify/default.aspx#ErrorMsg")
            {
                // This occurs if the license doesn't exist
                return (-1, new DateTime());
            }
            
            IWebElement expirationDateElement = _wait.Until(d => d.FindElement(By.Id("ExpirationDate")));
            // this delay is here because there was an exception while parsing on the next line if it ran too quickly
            Task.Delay(500).Wait();
            var expirationDate = DateTime.Parse(expirationDateElement.GetAttribute("innerHTML"));
            return (expirationDate.Subtract(CommonCode.Now).Days, expirationDate);
        }

        public void WriteExcelDocument(IEnumerable<Tradesman> tradesmen, string path)
        {
            _writer.WriteDataToFile(tradesmen, path);
        }
    }
}
