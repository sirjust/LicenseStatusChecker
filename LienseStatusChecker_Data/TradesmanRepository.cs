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
        IReader _reader;
        IWriter _writer;

        TradesmanRepository(IReader reader, IWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public bool WriteDataToFile(List<ITradesman> licenses, string path)
        {
            try
            {
                _writer.WriteDataToFile(licenses, path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<List<ITradesman>> ReadSpreadsheet(string location, string state)
        {
            return _reader.ReadSpreadSheet(location, state);
        }

        public int GetTradesmanCount(List<List<ITradesman>> tradesmen)
        {
            return _reader.GetTradesmanCount(tradesmen);
        }
    }
}
