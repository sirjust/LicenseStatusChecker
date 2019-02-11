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
            int counter = 0;
            var myFileInfo = new FileInfo(FilePaths.writePath);
            using (ExcelPackage package = new ExcelPackage(myFileInfo))
            {
                var worksheet = package.Workbook.Worksheets["mySheet"];
                int cellCounter = 1;
                while(counter < 10)
                {
                    foreach (Tradesman licenseHolder in licenses)
                    {
                        worksheet.Cells["A" + cellCounter].Value = licenseHolder.LicenseType;
                        worksheet.Cells["B" + cellCounter].Value = licenseHolder.LicenseNumber;
                        worksheet.Cells["C" + cellCounter].Value = licenseHolder.Name;
                        worksheet.Cells["D" + cellCounter].Value = licenseHolder.Address1;
                        worksheet.Cells["E" + cellCounter].Value = licenseHolder.Address2;
                        worksheet.Cells["F" + cellCounter].Value = licenseHolder.City;
                        worksheet.Cells["G" + cellCounter].Value = licenseHolder.Zip;
                        worksheet.Cells["H" + cellCounter].Value = licenseHolder.ExpirationDateFromSpreadSheet;
                        cellCounter++;
                        counter++;
                    }
                }
                package.Save();
            }
        }
    }
}
