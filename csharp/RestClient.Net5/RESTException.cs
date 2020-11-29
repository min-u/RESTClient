using System;
using System.Net;

namespace RestClient.Net5
{
    public class RESTException: Exception
    {
        public Response Response { get; private set; }
        public WebExceptionStatus WebExceptionStatus { get; private set; }

        internal RESTException(WebExceptionStatus webExceptionStatus, string message, Exception innerExceptoin = null, Response response = null)
            : base(message, innerExceptoin)
        {
            this.WebExceptionStatus = webExceptionStatus;
            this.Response = response;
        }
    }
}