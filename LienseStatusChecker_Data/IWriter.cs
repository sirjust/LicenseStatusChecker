using LicenseStatusChecker_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LienseStatusChecker_Data
{
    public interface IWriter
    {
        void WriteDataToFile(List<ITradesman> licenses, string path);
    }
}
