using LicenseStatusChecker_Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LienseStatusChecker_Data
{
    public class ExcelFileWriter : IWriter
    {
        public void WriteDataToFile(IEnumerable<ITradesman> licenses, string path)
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

        public void WriteSingleTradesmanToFile(ITradesman licenseHolder, string path)
        {
            try
            {
                var myFileInfo = new FileInfo(path);
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    var worksheet = package.Workbook.Worksheets.Count == 0 ? package.Workbook.Worksheets.Add("mySheet") : package.Workbook.Worksheets.Where(x => x.Name == "mySheet").FirstOrDefault();
                    int cellCounter = 1;
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells["A" + cellCounter].Value = "License Type";
                        worksheet.Cells["B" + cellCounter].Value = "License Number";
                        worksheet.Cells["C" + cellCounter].Value = "Name";
                        worksheet.Cells["D" + cellCounter].Value = "Address1";
                        worksheet.Cells["E" + cellCounter].Value = "Address2";
                        worksheet.Cells["F" + cellCounter].Value = "City";
                        worksheet.Cells["G" + cellCounter].Value = "State";
                        worksheet.Cells["H" + cellCounter].Value = "Zip";
                        worksheet.Cells["I" + cellCounter].Value = "ExpirationDate";
                    }
                    {
                        var dimension = worksheet.Dimension;
                        cellCounter = worksheet.Dimension.End.Row +1;
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
            catch(Exception ex)
            {
                Console.WriteLine($"There was an error writing to the excel file\n{ ex.Message}");
                throw;
            }
        }
    }
}
