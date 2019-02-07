using System;
using System.Collections.Generic;
using System.Linq;
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
                var hasHeader = false; // adjust accordingly '
                foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    tbl.Rows.Add(wsRow.Select(cell => cell.Text).ToArray());
                }
            }
            return tradesmen;
        }
    }
}
