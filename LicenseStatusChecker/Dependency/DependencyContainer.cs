using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;
using Ninject.Modules;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace LicenseStatusChecker.Dependency
{
    public class DependencyContainer : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger>();
            Bind<IReader>().To<ExcelFileReader>();
            Bind<IWriter>().To<ExcelFileWriter>();
            Bind<IWebDriver>().To<FirefoxDriver>();
            Bind<ILicenseChecker>().To<LicenseChecker>();
        }
    }
}
