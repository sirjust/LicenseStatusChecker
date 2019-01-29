using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public class CheckLicenses
    {
        IWebDriver driver;
        DateTime todaysDate = DateTime.Today;
        // string thisPath = System.IO.Directory.GetCurrentDirectory();

        public void inputLicenses(List<string> licenses)
        {
            // Console.WriteLine(thisPath);
            driver = new ChromeDriver(@"../../../packages/Selenium.Chrome.WebDriver.2.45/driver/");
            driver.Url = "https://secure.lni.wa.gov/verify/";
            driver.Manage().Window.Maximize();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            foreach (string license in licenses)
            {
                // c# adds another empty license at the end of the list.. this code prevents it from running into exceptions
                // we must be certain not to include empty spaces in between our licenses or it will only run to the first empty space
                if (String.IsNullOrEmpty(license))
                {
                    continue;
                }
                // we input the license into the page and search
                IWebElement searchDropdown = wait.Until<IWebElement>(d => d.FindElement(By.Id("selSearchType")));
                searchDropdown.Click();
                IWebElement selectByLicense = wait.Until<IWebElement>(d => d.FindElement(By.Id("license")));
                selectByLicense.Click();
                IWebElement textSearchBox = wait.Until<IWebElement>(d => d.FindElement(By.Id("txtSearchBy")));
                textSearchBox.SendKeys(license);
                IWebElement searchButton = wait.Until<IWebElement>(d => d.FindElement(By.Id("searchButton")));
                searchButton.Click();

                // now we pull up the details
                //IWebElement resultsArea = wait.Until<IWebElement>(d => d.FindElement(By.Id("resultsArea")));
                //resultsArea.Click();
                IWebElement resultsTable = wait.Until<IWebElement>(d => d.FindElement(By.ClassName("itemLink")));
                resultsTable.Click();

                // now we check the license's expiration date
                IWebElement expirationDateElement = wait.Until<IWebElement>(d => d.FindElement(By.Id("ExpirationDate")));
                DateTime expirationDate = DateTime.Parse(expirationDateElement.GetAttribute("innerHTML"));
                int daysTillExpiration = expirationDate.Subtract(todaysDate).Days;
                Console.WriteLine(daysTillExpiration.ToString());
                if(daysTillExpiration > 90) // if the expiration date is far in the future, the tradesman has already renewed
                {
                    continue;
                }

                // now we check if the license is currently valid
                IWebElement licenseValidity = wait.Until<IWebElement>(d => d.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[5]/div[2]/div[5]/div[4]/span[1]/strong[1]")));
                string isActive = licenseValidity.GetAttribute("innerHTML");
                if(isActive != "Active.")
                {
                    continue;
                }
            }
        }
    }
}
