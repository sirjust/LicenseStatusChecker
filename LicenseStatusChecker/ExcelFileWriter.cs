using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using LicenseStatusChecker_Common;

namespace LicenseStatusChecker
{
    class ExcelFileWriter
    {
        public void WriteDataToFile(List<ITradesman> licenses, string path)
        {
            var myFileInfo = new FileInfo(path);
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("mySheet");
                int cellCounter = 1;
                worksheet.Cells["A" + cellCounter].Value = "License Type";
                worksheet.Cells["B" + cellCounter].Value = "License Number";
                worksheet.Cells["C" + cellCounter].Value = "Name";
                worksheet.Cells["D" + cellCounter].Value = "Address1";
                worksheet.Cells["E" + cellCounter].Value = "Address2";
                worksheet.Cells["F" + cellCounter].Value = "City";
                worksheet.Cells["G" + cellCounter].Value = "State";
                worksheet.Cells["H" + cellCounter].Value = "Zip";
                worksheet.Cells["I" + cellCounter].Value = "ExpirationDate";
                cellCounter++;
                foreach (ITradesman licenseHolder in licenses)
                {
                    worksheet.Cells["A" + cellCounter].Value = licenseHolder.LicenseType;
                    worksheet.Cells["B" + cellCounter].Value = licenseHolder.LicenseNumber;
                    worksheet.Cells["C" + cellCounter].Value = licenseHolder.Name;
                    worksheet.Cells["D" + cellCounter].Value = licenseHolder.Address1;
                    worksheet.Cells["E" + cellCounter].Value = licenseHolder.Address2;
                    worksheet.Cells["F" + cellCounter].Value = licenseHolder.City;
                    worksheet.Cells["G" + cellCounter].Value = licenseHolder.State;
                    worksheet.Cells["H" + cellCounter].Value = licenseHolder.Zip;
                    worksheet.Cells["I" + cellCounter].Value = licenseHolder.ExpirationDate;
                    if (licenseHolder.NotSendReason != null)
                    {
                        worksheet.Cells["J" + cellCounter].Value = licenseHolder.NotSendReason;
                    }
                    cellCounter++;
                }
                try
                {
                    package.SaveAs(myFileInfo);
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("==================\n\nThe destination file appears to be open in another program. Please close it and run again.\n===============\n{0}", exception.ToString());
                }

            }
        }
    }
}
