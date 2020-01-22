using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public interface IWriter
    {
        void WriteDataToFile(List<ITradesman> licenses, string path);
    }
}
