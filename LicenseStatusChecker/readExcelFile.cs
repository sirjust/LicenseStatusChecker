using System;
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
        public List<Tradesman> readSpreadSheet(string spreadSheetLocation)
        {
            List<Tradesman> tradesmen = new List<Tradesman>();
            var pck = new OfficeOpenXml.ExcelPackage();
            pck.Load(new System.IO.FileInfo(FilePaths.readPath).OpenRead());
            if (pck.Workbook.Worksheets.Count != 0)
            {
                
                var sheet = pck.Workbook.Worksheets.First();
                var hasHeader = true; // adjust accordingly '
                //foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                //{
                //    Tradesman tradesman = new Tradesman();
                //    tradesman.LicenseType = (hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                //}
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    Tradesman tradesman = new Tradesman();
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    var rawData = wsRow.Value;
                    tradesman.ExpirationDateFromSpreadSheet = ((object[,])rawData)[0, 8].ToString();
                    DateTime expirationDate = DateTime.Parse(tradesman.ExpirationDateFromSpreadSheet);
                    int daysTillExpiration = expirationDate.Subtract(DateTime.Today).Days;
                    if (daysTillExpiration > 90)
                    {
                        continue;
                    }
                    tradesman.LicenseType = ((object[,])rawData)[0, 0].ToString();
                    tradesman.LicenseNumber = ((object[,])rawData)[0, 1].ToString();
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

                    tradesmen.Add(tradesman);
                }
            }
            return tradesmen;
        }
    }
}
