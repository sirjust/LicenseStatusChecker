﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public interface ITradesman
    {
        string LicenseType { get; set; }
        string LicenseNumber { get; set; }
        string Name { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        string ExpirationDate { get; set; }

        string Trade { get; set; }
        int HoursNeeded { get; set; }
        double HoursCompleted { get; set; }
        string NotSendReason { get; set; }
        int GetHoursNeeded(string trade);
    }
}
