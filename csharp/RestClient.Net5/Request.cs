using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using RestClient.Net5.Enums;

namespace RestClient.Net5
{
    public class Request: RequestBase
    {
        public static Request GetInstance { get; } = new Lazy<Request>(() => new Request(), true).Value;

        private Request()
        {
        }

        public Response Call(RequestInfo requestInfo)
        {
            try
            {
                if(requestInfo == default(RequestInfo))
                {
                    throw new ArgumentException("requestInfo is null (default)");
                }

                return this.CallHttpWebRequest(requestInfo);
            }
            catch(Exception ex) when (!(ex is ArgumentException) && !(ex is RESTException))
            {
                throw new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }

        private Response CallHttpWebRequest(RequestInfo requestInfo)
        {
            try
            {
                HttpWebRequest httpWebRequest = this.GetHttpWebRequest(requestInfo);
                var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                
                return MakeResponse(httpWebResponse, requestInfo);
            }
            catch(WebException exWeb)
            {
                HttpWebResponse webResponse = (HttpWebResponse) exWeb.Response;
                if(webResponse != null)
                {
                    Response res = MakeResponse((HttpWebResponse) exWeb.Response, requestInfo);
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
            catch(Exception ex)
            {
                throw new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }
    }
}