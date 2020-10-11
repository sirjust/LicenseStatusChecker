using LicenseStatusChecker.Dependency;
using LicenseStatusChecker_Common;
using LienseStatusChecker_Data;
using Ninject;

namespace LicenseStatusChecker
{
    class Program
    {
        static void Main()
        {
            var kernel = new StandardKernel(new DependencyContainer());

            kernel.Get<ILogger>().LogStart();
            kernel.Get<ILicenseChecker>().CheckLicenses(kernel.Get<IReader>().ReadSpreadSheet(SharedFilePaths.readPath, "WA"));
            kernel.Get<ILogger>().LogEnd();
        }
    }
}