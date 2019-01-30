using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseStatusChecker
{
    public class Tradesman
    {
        public string LicenseNumber { get; set; }
        public string Trade { get; set; }
        public int HoursNeeded { get; set; }

        public int GetHoursNeeded(string trade)
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
