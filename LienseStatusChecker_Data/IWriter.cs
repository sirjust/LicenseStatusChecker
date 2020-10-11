using LicenseStatusChecker_Common;
using System.Collections.Generic;

namespace LienseStatusChecker_Data
{
    public interface IWriter
    {
        void WriteDataToFile(IEnumerable<ITradesman> licenses, string path);
        void WriteSingleTradesmanToFile(ITradesman tradesman, string path);
    }
}
