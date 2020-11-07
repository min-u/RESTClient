using System;
using System.Net;

namespace RESTClient
{
    public class RestException: Exception
    {
        public Response ExceptonResponse { get; private set; }
        public WebExceptionStatus ExceptoinStatus { get; private set; }

        internal RestException(WebExceptionStatus webExceptionStatus, string message, Exception innerExceptoin = null, Response exceptonResponse = null)
            : base(message, innerExceptoin)
        {
            this.ExceptoinStatus = webExceptionStatus;
            this.ExceptonResponse = exceptonResponse;
        }
    }
}
