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

namespace LicenseStatusChecker
{
    public class CheckLicenses
    {
        IWebDriver driver;
        DateTime todaysDate = DateTime.Today;
        // string thisPath = System.IO.Directory.GetCurrentDirectory();

        public List<Tradesman> inputLicenses(List<Tradesman> tradesmanList)
        {
            // Console.WriteLine(thisPath);
            List<Tradesman> tradesmenToSend = new List<Tradesman>();
            driver = new ChromeDriver(@"../../../packages/Selenium.Chrome.WebDriver.2.45/driver/");
            driver.Url = "https://secure.lni.wa.gov/verify/";
            driver.Manage().Window.Maximize();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            foreach (Tradesman thisTradesman in tradesmanList)
            {
                // this sleep is here to allow time to change the data, otherwise there is a chance the same license could be entered twice and someone could be skipped
                Task.Delay(1000).Wait();

                // we input the license into the page and search
                IWebElement searchDropdown = wait.Until<IWebElement>(d => d.FindElement(By.Id("selSearchType")));
                searchDropdown.Click();
                IWebElement selectByLicense = wait.Until<IWebElement>(d => d.FindElement(By.Id("license")));
                selectByLicense.Click();
                IWebElement textSearchBox = wait.Until<IWebElement>(d => d.FindElement(By.Id("txtSearchBy")));
                textSearchBox.SendKeys(thisTradesman.LicenseNumber);
                IWebElement searchButton = wait.Until<IWebElement>(d => d.FindElement(By.Id("searchButton")));
                searchButton.Click();

                // now we pull up the details
                IWebElement resultsTable = wait.Until<IWebElement>(d => d.FindElement(By.ClassName("itemLink")));
                resultsTable.Click();

                // now we check the license's expiration date
                IWebElement expirationDateElement = wait.Until<IWebElement>(d => d.FindElement(By.Id("ExpirationDate")));
                // this delay is here because there was an exception while parsing on the next line if it ran too quickly
                Task.Delay(1000).Wait();
                DateTime expirationDate = DateTime.Parse(expirationDateElement.GetAttribute("innerHTML"));
                int daysTillExpiration = expirationDate.Subtract(todaysDate).Days;
                // Console.WriteLine("{0}: {1}", license, daysTillExpiration.ToString());
                IWebElement backButton = wait.Until(d => d.FindElement(By.Id("backBtn")));
                if (daysTillExpiration > 90) // if the expiration date is far in the future, the tradesman has already renewed
                {
                    Console.WriteLine("{0} has likely already renewed.", thisTradesman.LicenseNumber);
                    backButton.Click();
                    continue;
                }

                // now we check if the license is currently valid
                IWebElement licenseValidity = wait.Until<IWebElement>(d => d.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[5]/div[2]/div[5]/div[4]/span[1]/strong[1]")));
                string isActive = licenseValidity.GetAttribute("innerHTML");
                if(isActive != "Active.")
                {
                    // if the license is expired or inactive, we move on
                    Console.WriteLine("{0} is not active.", thisTradesman.LicenseNumber);
                    backButton.Click();
                    continue;
                }

                // we need to check the trade and rank of the license holder
                IWebElement licenseTypeElement = wait.Until<IWebElement>(d => d.FindElement(By.Id("LicenseType")));
                thisTradesman.Trade = licenseTypeElement.GetAttribute("innerHTML");
                thisTradesman.HoursNeeded = thisTradesman.GetHoursNeeded(thisTradesman.Trade);

                // now that all our properties are populated, we need to check them against the credits the tradesman has already completed
                bool hasCourses;
                try
                {
                    IWebElement coursesElement = wait.Until(d => d.FindElement(By.Id("Courses")));
                    hasCourses = true;
                }
                catch
                {
                    hasCourses = false;
                }
                if (!hasCourses)
                {
                    // add license to the send list
                    backButton.Click();
                    continue;
                }
                var allDataItems = driver.FindElements(By.XPath("//span[contains(text(),'.00')]"));
                List<string> coursesTaken = new List<string>();
                double numberOfCredits = 0.0;
                foreach (var item in allDataItems)
                {
                    string data = item.GetAttribute("innerHTML");
                    int index = data.IndexOf(" ");
                    if (index > 0)
                        data = data.Substring(0, index);
                    coursesTaken.Add(data);
                    numberOfCredits += Convert.ToDouble(data);
                }
                Console.WriteLine("{0} has completed {1} credits and needs {2}", thisTradesman.LicenseNumber, numberOfCredits, thisTradesman.HoursNeeded);
                if (thisTradesman.HoursNeeded > numberOfCredits)
                {
                    tradesmenToSend.Add(thisTradesman);
                    //RecordLicensesToSend recordThis = new RecordLicensesToSend();
                    //StreamWriter sw = new StreamWriter(@"licensesToSend.txt", true);
                    //recordThis.record(thisTradesman.LicenseNumber, sw);
                    //sw.Close();
                }
                backButton.Click();
            }
            return tradesmenToSend;
        }
    }
}
