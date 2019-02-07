using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace LicenseStatusChecker
{
    class WriteToExcelFile
    {
        public void WriteDataToFile(List<Tradesman> licenses)
        {
            var myFileInfo = new FileInfo(FilePaths.writePath);
            using (ExcelPackage package = new ExcelPackage(myFileInfo))
            {
                var worksheet = package.Workbook.Worksheets["mySheet"];
                int cellCounter = 1;
                foreach(Tradesman licenseHolder in licenses)
                {
                    worksheet.Cells["A"+cellCounter].Value = licenseHolder.LicenseNumber;
                    cellCounter++;
                }
                package.Save();
            }
        }

        // the readXLS function didn't work, presumably because my file is .ods
    }
}
