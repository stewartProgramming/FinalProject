using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    public class DateTimeService
    {
        public static DateTime GetTimeNow()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime custDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, myZone);
            return custDateTime;
        }
    }
}
