using LicenseStatusChecker_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LienseStatusChecker_Data
{
    public interface IReader
    {
        List<List<ITradesman>> ReadSpreadSheet(string spreadSheetLocation);

        int GetTradesmanCount(List<List<ITradesman>> tradesmen);
    }
}
