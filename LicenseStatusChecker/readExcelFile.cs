using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AODL;
using AODL.Document.SpreadsheetDocuments;

namespace LicenseStatusChecker
{
    public class readExcelFile
    {
        public List<List<Tradesman>> readSpreadSheet(string spreadSheetLocation)
        {
            List<List<Tradesman>> ListOfTradesmen = new List<List<Tradesman>>(5);
            var pck = new OfficeOpenXml.ExcelPackage();
            pck.Load(new System.IO.FileInfo(FilePaths.readPath).OpenRead());
            if (pck.Workbook.Worksheets.Count != 0)
            {
                int worksheetCount = pck.Workbook.Worksheets.Count;
                for(int i= 1; i<= worksheetCount; i++)
                {
                    // the second list is not yet instantiated, here we instantiate one for each sheet
                    ListOfTradesmen.Add(new List<Tradesman>());
                    var sheet = pck.Workbook.Worksheets[i];
                    var hasHeader = true;
                    var startRow = hasHeader ? 2 : 1;
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
                            Logger logger = new Logger();
                            StreamWriter sw = new StreamWriter(@"exceptionLog.txt", true);
                            logger.writeErrorsToLog($"{ex}\n" + "This tradesman has either no license number or no expiration date.", sw);
                            sw.Close();
                            continue;
                        }
                        DateTime expirationDate = DateTime.Parse(tradesman.ExpirationDate);
                        int daysTillExpiration = expirationDate.Subtract(DateTime.Today).Days;
                        if (daysTillExpiration > 90)
                        {
                            // log that this tradesman will not have the license checked
                            Logger logger = new Logger();
                            StreamWriter sw = new StreamWriter(@"greaterThan90.txt", true);
                            logger.writeErrorsToLog($"{tradesman.LicenseNumber}'s expiration date is greater than 90.", sw);
                            sw.Close();
                            continue;
                        }
                        if (daysTillExpiration < 0)
                        {
                            // log that this tradesman will not have the license checked
                            Logger logger = new Logger();
                            StreamWriter sw = new StreamWriter(@"expired.txt", true);
                            logger.writeErrorsToLog($"{tradesman.LicenseNumber}'s expiration date is in the past.", sw);
                            sw.Close();
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

                            ListOfTradesmen[i-1].Add(tradesman);
                        }
                        catch (NullReferenceException ex)
                        {
                            Logger logger = new Logger();
                            StreamWriter sw = new StreamWriter(@"exceptionLog.txt", true);
                            logger.writeErrorsToLog($"{ex}\n" + $"{tradesman.LicenseNumber}'s record has null or incorrect values.", sw);
                            sw.Close();
                            continue;
                        }
                    }
                }
            }
            return ListOfTradesmen;
        }
    }
}
