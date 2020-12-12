using System;
using System.Net;

namespace RESTClient
{
    public sealed class Request: RequestBase
    {
        public static Request GetInstance { get; } = new Lazy<Request>(() => new Request(), true).Value;

        private Request()
        {
        }

        public Response Call(RequestInfo requestInfo)
        {
            if(requestInfo == default(RequestInfo))
                throw new ArgumentException("requestInfo is null (default)");

            try
            {
                return this.CallHttpWebRequest(requestInfo);
            }
            catch(Exception ex) when(!(ex is ArgumentException) && !(ex is RESTException))
            {
                throw new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }

        private Response CallHttpWebRequest(RequestInfo requestInfo)
        {
            try
            {
                var httpWebRequest = this.GetHttpWebRequest(requestInfo);
                var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;

                return MakeResponse(httpWebResponse, requestInfo.Encoding);
            }
            catch(WebException exWeb)
            {
                var webResponse = (HttpWebResponse) exWeb.Response;
                if(webResponse != null)
                {
                    var res = MakeResponse((HttpWebResponse) exWeb.Response, requestInfo.Encoding);
                    if(res.StatusCode != HttpStatusCode.OK && requestInfo.ThrowRestExceptionWhenStatusNotOK)
                    {
                        throw new RESTException(exWeb.Status, exWeb.Message, exWeb, res);
                    }
                    return res;
                }
                else
                {
                    throw new RESTException(exWeb.Status, exWeb.Message, exWeb);
                }
            }
            catch(Exception ex) when(!(ex is RESTException))
            {
                throw new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }
    }
}