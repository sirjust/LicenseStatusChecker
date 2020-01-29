using LicenseStatusChecker_Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LienseStatusChecker_Data
{
    public class ExcelFileReader : IReader
    {
        int errorCount { get; set; }
        ILogger _logger;
        public ExcelFileReader(ILogger logger)
        {
            _logger = logger;
            errorCount = 0;
        }
        public List<List<ITradesman>> ReadSpreadSheet(string spreadSheetLocation, string state)
        {
            _logger.WriteToConsole("The program is now reading the spreadsheet.\n-----");
            List<List<ITradesman>> listOfTradesmen = new List<List<ITradesman>>(5);
            var pck = new OfficeOpenXml.ExcelPackage();
            pck.Load(new System.IO.FileInfo(SharedFilePaths.readPath).OpenRead());
            if (pck.Workbook.Worksheets.Count != 0)
            {
                int worksheetCount = pck.Workbook.Worksheets.Count;
                // The OpenXML library is 1-based in this environment
                for (int i = 1; i <= worksheetCount; i++)
                {
                    // the second list is not yet instantiated, here we instantiate one for each sheet
                    listOfTradesmen.Add(new List<ITradesman>());
                    var sheet = pck.Workbook.Worksheets[i];

                    var hasHeader = true;
                    var startRow = hasHeader ? 2 : 1;
                    if (sheet.Dimension == null) continue;
                    if(state == "WA")
                    {
                        ReadWashingtonSheet(listOfTradesmen, sheet, startRow, i);
                    }
                    else
                    {
                        ReadOregonSheet(listOfTradesmen, sheet, startRow, i);
                    }
                }
            }
            int count = GetTradesmanCount(listOfTradesmen);
            _logger.WriteToConsole($"The program has finished reading the spreadsheet.\nThere were {errorCount} error(s), recorded in the logs.\nIt will now check {count} license(s).\n-----");
            return listOfTradesmen;
        }

        private void ReadOregonSheet(List<List<ITradesman>> listOfTradesmen, ExcelWorksheet sheet, int startRow, int i)
        {
            for(var rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
            {
                Tradesman tradesman = new Tradesman();
                var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                var rawData = wsRow.Value;
                try
                {
                    tradesman.LicenseNumber = ((object[,])rawData)[0,0].ToString();
                }
                catch (NullReferenceException ex)
                {
                    string message = $"{ex}\n" + "This tradesman has no license number.";
                    _logger.WriteErrorsToLog(message, SharedFilePaths.exceptionLog);
                    errorCount++;
                    continue;
                }
                tradesman.LicenseType = ((object[,])rawData)[0, 1].ToString();
                var firstName = ((object[,])rawData)[0, 2].ToString();
                var lastName = ((object[,])rawData)[0, 3].ToString();
                tradesman.Name = $"{firstName} {lastName}";
                if (((object[,])rawData)[0, 4] != null)
                {
                    tradesman.Address1 = ((object[,])rawData)[0, 4].ToString();
                }
                tradesman.City = ((object[,])rawData)[0, 5].ToString();
                if (((object[,])rawData)[0, 6] != null)
                {
                    tradesman.State = ((object[,])rawData)[0, 6].ToString();
                }

                tradesman.Zip = ((object[,])rawData)[0, 7].ToString();

                listOfTradesmen[i - 1].Add(tradesman);
            }
        }

        public int GetTradesmanCount(List<List<ITradesman>> tradesmen)
        {
            int counter = 0;
            foreach (List<ITradesman> list in tradesmen)
            {
                counter += list.Count;
            }
            return counter;
        }

        public void ReadWashingtonSheet(List<List<ITradesman>> listOfTradesmen, OfficeOpenXml.ExcelWorksheet sheet, int startRow, int i)
        {
            for (var rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
            {
                Tradesman tradesman = new Tradesman();
                var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                var rawData = wsRow.Value;
                try
                {
                    tradesman.LicenseNumber = ((object[,])rawData)[0, 1].ToString();
                    tradesman.ExpirationDate = ((object[,])rawData)[0, 8].ToString();
                }
                catch (NullReferenceException ex)
                {
                    string message = $"{ex}\n" + "This tradesman has either no license number or no expiration date.";
                    _logger.WriteErrorsToLog(message, SharedFilePaths.exceptionLog);
                    errorCount++;
                    continue;
                }
                DateTime expirationDate = DateTime.Parse(tradesman.ExpirationDate);
                int daysTillExpiration = expirationDate.Subtract(CommonCode.Now).Days;
                if (daysTillExpiration > 90)
                {
                    // log that this tradesman will not have the license checked
                    var message = $"{tradesman.LicenseNumber}'s expiration date is greater than 90.";
                    _logger.WriteErrorsToLog(message, SharedFilePaths.greaterThan90Log);
                    errorCount++;
                    continue;
                }
                if (daysTillExpiration < 0)
                {
                    // log that this tradesman will not have the license checked
                    var message = $"{tradesman.LicenseNumber}'s expiration date is in the past.";
                    _logger.WriteErrorsToLog(message, SharedFilePaths.expiredLog);
                    errorCount++;
                    continue;
                }
                try
                {
                    tradesman.LicenseType = ((object[,])rawData)[0, 0].ToString();
                    tradesman.Name = ((object[,])rawData)[0, 2].ToString();
                    tradesman.Address1 = ((object[,])rawData)[0, 3].ToString();
                    if (((object[,])rawData)[0, 4] != null)
                    {
                        tradesman.Address2 = ((object[,])rawData)[0, 4].ToString();
                    }
                    tradesman.City = ((object[,])rawData)[0, 5].ToString();
                    if (((object[,])rawData)[0, 6] != null)
                    {
                        tradesman.State = ((object[,])rawData)[0, 6].ToString();
                    }

                    tradesman.Zip = ((object[,])rawData)[0, 7].ToString();

                    listOfTradesmen[i - 1].Add(tradesman);
                }
                catch (NullReferenceException ex)
                {
                    var message = $"{ex}\n" + $"{tradesman.LicenseNumber}'s record has null or incorrect values.";
                    _logger.WriteErrorsToLog(message, SharedFilePaths.exceptionLog);
                    errorCount++;
                    continue;
                }
            }
        }

        public void ReadOregonSheet()
        {

        }
    }
}
