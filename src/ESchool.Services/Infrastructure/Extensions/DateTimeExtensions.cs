using System;

namespace ESchool.Services.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime datetime)
        {
            return datetime.Date;
        }

        public static DateTime EndOfDay(this DateTime datetime)
        {
            return datetime.Date.AddDays(1).AddTicks(-1);
        }
    }
}
