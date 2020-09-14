using System;
using System.Globalization;

namespace src.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime ParseDate(string date, string timeFormat)
        {
            if (DateTime.TryParseExact(date, timeFormat, null,
                DateTimeStyles.AdjustToUniversal, out var newDate))
            {
            }
            else
            {
                Console.WriteLine("Error occured at this line: '{0}'", date);
                Console.WriteLine("Unable to parse '{0}'", timeFormat);
            }

            return newDate;
        }
    }
}