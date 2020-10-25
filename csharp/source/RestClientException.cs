using System;
using System.Net;

namespace RESTClient
{
    public class RestClientException: Exception
    {
        public Response ExceptonResponse { get; private set; }
        public WebExceptionStatus ExceptoinStatus { get; private set; }

        internal RestClientException(WebExceptionStatus webExceptionStatus, string message, Exception innerExceptoin, Response exceptonResponse = null)
            : base(message, innerExceptoin)
        {
            this.ExceptoinStatus = webExceptionStatus;
            this.ExceptonResponse = exceptonResponse;
        }
    }
}
