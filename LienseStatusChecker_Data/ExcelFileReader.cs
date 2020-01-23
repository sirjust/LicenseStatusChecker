using LicenseStatusChecker_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LienseStatusChecker_Data
{
    public class ExcelFileReader
    {
        int errorCount { get; set; }
        ILogger _logger;
        public ExcelFileReader(ILogger logger)
        {
            _logger = logger;
            errorCount = 0;
        }
        public List<List<Tradesman>> ReadSpreadSheet(string spreadSheetLocation)
        {
            _logger.WriteToConsole("The program is now reading the spreadsheet.\n-----");
            List<List<Tradesman>> ListOfTradesmen = new List<List<Tradesman>>(5);
            var pck = new OfficeOpenXml.ExcelPackage();
            pck.Load(new System.IO.FileInfo(SharedFilePaths.readPath).OpenRead());
            if (pck.Workbook.Worksheets.Count != 0)
            {
                int worksheetCount = pck.Workbook.Worksheets.Count;
                // The OpenXML library is 1-based in this environment
                for (int i = 1; i <= worksheetCount; i++)
                {
                    // the second list is not yet instantiated, here we instantiate one for each sheet
                    ListOfTradesmen.Add(new List<Tradesman>());
                    var sheet = pck.Workbook.Worksheets[i];

                    var hasHeader = true;
                    var startRow = hasHeader ? 2 : 1;
                    if (sheet.Dimension == null) continue;
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
                        int daysTillExpiration = expirationDate.Subtract(DateTime.Today).Days;
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

                            ListOfTradesmen[i - 1].Add(tradesman);
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
            }
            int count = GetTradesmanCount(ListOfTradesmen);
            _logger.WriteToConsole($"The program has finished reading the spreadsheet.\nThere were {errorCount} error(s), recorded in the logs.\nIt will now check {count} license(s).\n-----");
            return ListOfTradesmen;
        }
        private int GetTradesmanCount(List<List<Tradesman>> tradesmen)
        {
            int counter = 0;
            foreach (List<Tradesman> list in tradesmen)
            {
                counter += list.Count;
            }
            return counter;
        }
    }
}
