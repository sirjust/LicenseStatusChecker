using LicenseStatusChecker_Common;

namespace LicenseStatusChecker
{
    public class WashingtonTradesman : Tradesman, ITradesman
    {
        public override int GetHoursNeeded(string trade)
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
