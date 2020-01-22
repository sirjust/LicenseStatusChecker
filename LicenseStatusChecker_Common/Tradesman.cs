using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker_Common
{
    public class Tradesman : ITradesman
    {
        // these properties are imported from the spreadsheet
        public string LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ExpirationDate { get; set; }

        // we will verify trade in the web application
        public string Trade { get; set; }
        public int HoursNeeded { get; set; }
        public double HoursCompleted { get; set; }
        public string NotSendReason { get; set; }

        public virtual int GetHoursNeeded(string trade)
        {
            int hoursNeeded;
            switch (trade)
            {
                case "Plumber Trainee":
                    hoursNeeded = 8;
                    break;
                case "Plumber":
                    hoursNeeded = 16;
                    break;
                case "Electrician":
                    hoursNeeded = 24;
                    break;
                default:
                    hoursNeeded = 0;
                    break;
            }
            return hoursNeeded;
        }
    }
}
