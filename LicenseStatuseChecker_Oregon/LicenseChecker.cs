using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LicenseStatuseChecker_Oregon
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
            int userInput = 0;
            int iterations = 0;
            int successfulRetrieves = 0;

            foreach (List<ITradesman> tradeList in _tradesmen)
            {
                foreach (ITradesman OregonTradesman in tradeList)
                {
                    try
                    {
                        _driver.Url = $"http://www4.cbs.state.or.us/ex/all/mylicsearch/index.cfm?fuseaction=search.show_search_name&group_id=30";
                        
                        // this should give the user enough time to click the button
                        if(iterations % 11 == 0)
                        {
                            Console.Beep();
                            Thread.Sleep(60000);
                            userInput++;
                        }
                        var licenseInput = _driver.FindElement(By.Name("search_licno"));
                        licenseInput.SendKeys(OregonTradesman.LicenseNumber);

                        var submit = _driver.FindElement(By.Name("submit"));
                        submit.Click();

                        successfulRetrieves++;
                        iterations++;
                        Console.WriteLine($"The program has fetched data {successfulRetrieves} times.");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("There was a {0} with {1}.", ex.Message, OregonTradesman.LicenseNumber);
                        _logger.WriteErrorsToLog($"{ex}\n" + "There was a Selenium error.", SharedFilePaths.exceptionLog);
                        iterations++;
                        continue;
                    }
                }
                _writer.WriteDataToFile(doNotSend, SharedFilePaths.doNotSendPath);
            }

            return tradesmenToSend;
        }
    }
}
