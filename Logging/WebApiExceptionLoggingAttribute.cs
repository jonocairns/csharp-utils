    /// <summary>
    /// Custom error logging for web api controllers. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), AttributeUsage(AttributeTargets.Class)]
    public sealed class WebApiExceptionLoggingAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Obrien.Connect.Common.ILogger.Log(System.String,System.Exception)")]
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Argument.CheckIfNull(actionExecutedContext, "actionExecutedContext");

            RaygunLogger logger = new RaygunLogger(new ConnectVersion());
            logger.Log("Web API Error", actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }