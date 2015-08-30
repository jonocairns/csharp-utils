    /// <summary>
    /// Responsible for logging messages and exceptions.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        void Log(Exception exception);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        void Log(string message);

        /// <summary>
        /// Log any message and its associated exception.
        /// </summary>
        void Log(string message, Exception exception);

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        void Trace(string message);
    }