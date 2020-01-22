using LicenseStatusChecker_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LienseStatusChecker_Data
{
    public class TradesmanRepository
    {
        List<ITradesman> Tradesmen;

        public void WriteDataToFile(List<ITradesman> licenses, string path)
        {
            // TODO call appropriate writer
        }

        public List<List<ITradesman>> ReadSpreadsheet(string location)
        {
            throw new NotImplementedException();
        }

        private int GetTradesmanCount(List<List<ITradesman>> tradesmen)
        {
            throw new NotImplementedException();
        }
    }
}
