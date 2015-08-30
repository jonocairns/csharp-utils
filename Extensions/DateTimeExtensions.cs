 public static class DateTimeExtensions
    {
        public static string ToCommonFormat(this DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return string.Empty;
            }

            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        public static DateTime ToEndOfDay(this DateTime value)
        {
            DateTime beginingOfToday = value.Date;
            DateTime beginingOfTomorrow = beginingOfToday.Date.AddDays(1);
            return beginingOfTomorrow.AddTicks(-1);
        }

        public static DateTime ToStartOfDay(this DateTime value)
        {
            return value.Date;
        }
    }