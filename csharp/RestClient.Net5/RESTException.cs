using System;
using System.Net;

namespace RestClient.Net5
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