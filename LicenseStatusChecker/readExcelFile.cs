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
                    tradesman.LicenseType = ((object[,])rawData)[0, 0].ToString();
                    tradesman.LicenseNumber = ((object[,])rawData)[0, 1].ToString();

                    tradesmen.Add(tradesman);
                }
            }
            return tradesmen;
        }
    }
}
