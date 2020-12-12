using System;
using System.Net;

namespace RESTClient
{
    public class RESTException: Exception
    {
        public Response Response { get; init; }
        public WebExceptionStatus WebExceptionStatus { get; init; }

        internal RESTException(WebExceptionStatus webExceptionStatus, string message = "", Exception innerExceptoin = null, Response response = null)
            : base(message, innerExceptoin)
        {
            this.WebExceptionStatus = webExceptionStatus;
            this.Response = response;
        }
    }
}