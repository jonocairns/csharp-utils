 /// <summary>
    /// Handling concurrency errors.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1018:MarkAttributesWithAttributeUsage")]
    public sealed class WebApiConcurrencyExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Argument.CheckIfNull(actionExecutedContext, "actionExecutedContext");

            if (actionExecutedContext.Exception.GetType() == typeof(ConcurrencyException))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(actionExecutedContext.Exception.ToString())
                };

                actionExecutedContext.Response = response;
            }
            base.OnException(actionExecutedContext);
        }
    }