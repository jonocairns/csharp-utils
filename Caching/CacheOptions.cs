 public class CacheOptions
    {
        private CacheOptions(int days, int hours, int minutes, int seconds)
        {
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public int Days { get; private set; }
        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public DateTime Expiry
        {
            get
            {
                return DateTime.UtcNow
                    .AddDays(Days)
                    .AddHours(Hours)
                    .AddMinutes(Minutes)
                    .AddSeconds(Seconds);
            }
        }

        public static CacheOptions FromSeconds(int seconds)
        {
            return new CacheOptions(0, 0, 0, seconds);
        }

        public static CacheOptions FromMinutes(int minutes)
        {
            return new CacheOptions(0, 0, minutes, 0);
        }

        public static CacheOptions FromHours(int hours)
        {
            return new CacheOptions(0, hours, 0, 0);
        }

        #region Equality

        protected bool Equals(CacheOptions other)
        {
            Argument.CheckIfNull(other, "other");
            return Days == other.Days && Hours == other.Hours && Minutes == other.Minutes && Seconds == other.Seconds;
        }

        public override bool Equals(object obj)
        {
            Argument.CheckIfNull(obj, "obj");
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CacheOptions)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Days;
                hashCode = (hashCode * 397) ^ Hours;
                hashCode = (hashCode * 397) ^ Minutes;
                hashCode = (hashCode * 397) ^ Seconds;
                return hashCode;
            }
        }

        public static bool operator ==(CacheOptions left, CacheOptions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CacheOptions left, CacheOptions right)
        {
            return !Equals(left, right);
        }


        #endregion
    }