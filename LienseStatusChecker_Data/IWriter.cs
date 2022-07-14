using LicenseStatusChecker_Common;
using System.Collections.Generic;

namespace LienseStatusChecker_Data
{
    public interface IWriter
    {
        void WriteAlreadyTakenCourses(Dictionary<string, int> courses);
        void WriteDataToFile(IEnumerable<ITradesman> licenses, string path);
        void WriteSingleTradesmanToFile(ITradesman tradesman, string path);
    }
}
