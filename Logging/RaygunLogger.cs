/// <summary>
    /// Responsible for logging messages and exceptions using Raygun.io.
    /// </summary>
    public class RaygunLogger : ILogger
    {
        private readonly ConnectVersion _connectVersion;
        private readonly RaygunClient _raygunLoggingClient;
        private readonly RaygunClient _raygunTracingClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RaygunLogger"/> class.
        /// </summary>
        public RaygunLogger(ConnectVersion connectVersion)
        {
            Argument.CheckIfNull(connectVersion, "connectVersion");
            
            _connectVersion = connectVersion;
            
            Setting loggingKey = Settings.Load("raygunLoggingApiKey");
            _raygunLoggingClient = new RaygunClient(loggingKey.ToString());
            

            Setting tracingKey = Settings.Load("raygunTracingApiKey");
            _raygunTracingClient = new RaygunClient(tracingKey.ToString());

        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        public void Log(Exception exception)
        {
            Argument.CheckIfNull(exception, "exception");
            _raygunLoggingClient.SendInBackground(exception);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        public void Log(string message)
        {
            Argument.CheckIfNull(message, "message");

            Log(message, new ConnectException(message));
        }

        /// <summary>
        /// Log any message and its associated exception.
        /// </summary>
        public void Log(string message, Exception exception)
        {
            Argument.CheckIfNull(message, "message");
            Argument.CheckIfNull(exception, "exception");

            Dictionary<string, string> customData = new Dictionary<string, string>();
            customData.Add("message", message);
            customData.Add("version", _connectVersion.FullVersion);
            _raygunLoggingClient.SendInBackground(exception, new List<string>(), customData);
        }

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        public void Trace(string message)
        {
            Argument.CheckIfNull(message, "message");
            
            Dictionary<string, string> customData = new Dictionary<string, string>
            {
                {"message", message},
                {"version", _connectVersion.FullVersion}
            };
            _raygunTracingClient.SendInBackground(new ConnectException(message), new List<string>(), customData);
        }
    }