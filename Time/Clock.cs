 /// <summary>
    /// Get the clock.
    /// </summary>
    public class Clock : IClock
    {
        /// <summary>
        /// Gets the current time
        /// </summary>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets the current time in UTC format
        /// </summary>
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        /// <summary>
        /// Gets the current date without a time component
        /// </summary>
        public DateTime Today
        {
            get { return DateTime.Today; }
        }
    }