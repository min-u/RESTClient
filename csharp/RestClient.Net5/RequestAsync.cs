using System;
using System.Net;
using System.Threading.Tasks;

namespace RESTClient
{
    public sealed class RequestAsync: RequestBase
    {
        public static RequestAsync GetInstance { get; } = new Lazy<RequestAsync>(() => new RequestAsync(), true).Value;

        private RequestAsync()
        {
        }

        public async Task<Response> CallAsync(RequestInfo requestInfo)
        {
            if(requestInfo == default(RequestInfo))
                throw new ArgumentException("requestInfo is null (default)");

            try
            {
                return await this.CallHttpWebRequestAsync(requestInfo);
            }
            catch(Exception ex) when(!(ex is ArgumentException) && !(ex is RESTException))
            {
                throw new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }

        private async Task<Response> CallHttpWebRequestAsync(RequestInfo requestInfo)
        {
            try
            {
                var httpWebRequest = await this.GetHttpWebRequestAsync(requestInfo);
                var httpWebResponse = await httpWebRequest.GetResponseAsync() as HttpWebResponse;

                return await MakeResponseAsync(httpWebResponse, requestInfo.Encoding);
            }
            catch(WebException exWeb)
            {
                HttpWebResponse webResponse = (HttpWebResponse) exWeb.Response;
                if(webResponse != null)
                {
                    Response res = await MakeResponseAsync((HttpWebResponse) exWeb.Response, requestInfo.Encoding);
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