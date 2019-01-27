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
        // string thisPath = System.IO.Directory.GetCurrentDirectory();

        public void inputLicenses(List<string> licenses)
        {
            // Console.WriteLine(thisPath);
            driver = new ChromeDriver(@"../../../packages/Selenium.Chrome.WebDriver.2.43/driver/");
            driver.Url = "https://secure.lni.wa.gov/verify/";
            driver.Manage().Window.Maximize();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));


            foreach (string license in licenses)
            {

            }
        }
    }
}
