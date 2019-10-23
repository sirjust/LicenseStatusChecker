﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public static class FilePaths
    {
        private static string formattedDate = $"{DateTime.Today.Month.ToString()}-{DateTime.Today.Day.ToString()}-{DateTime.Today.Year.ToString()}";
        public static string readPath = $"..\\..\\..\\Files\\listToCheck.xlsx";
        public static string sendPath = $"..\\..\\..\\Files\\Send-{formattedDate}.xlsx";
        public static string doNotSendPath = $"..\\..\\..\\Files\\DoNotSend-{formattedDate}.xlsx";
    }
}
