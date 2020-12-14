
using RESTClient.Enum;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RESTClient
{
    public static class Request
    {
        public static RequestResult Call(RequestInfo requestInfo)
        {
            var callResult = new RequestResult();
            try
            {
                callResult.Response = CalHttpWebRequest(requestInfo);
            }
            catch(RESTException restException)
            {
                callResult.Exception = restException;
            }
            catch(Exception ex)
            {
                callResult.Exception = new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }

            return callResult;
        }

        private static Response CalHttpWebRequest(RequestInfo requestInfo)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(requestInfo.GetURI());
                webRequest.Method = requestInfo.Method.GetName();
                webRequest.ContentType = requestInfo.RequestDataType.GetContentType();
                webRequest.Timeout = requestInfo.TimeoutSecond * 1000;
                webRequest.ContinueTimeout = requestInfo.ContinueTimeoutSeconds * 1000;

                if(requestInfo.Proxy == default)
                    webRequest.Proxy = requestInfo.Proxy;

                foreach(var keyValue in requestInfo.GetHeader())
                    webRequest.Headers.Add(keyValue.Key, keyValue.Value);

                switch(requestInfo.Method)
                {
                    case HttpMethod.PATCH:
                    case HttpMethod.POST:
                    case HttpMethod.PUT:
                    {
                        byte[] buffer = requestInfo.GetBodyBytes();
                        webRequest.ContentLength = buffer.Length;

                        using(Stream dataStream = webRequest.GetRequestStream())
                        {
                            dataStream.Write(buffer, 0, buffer.Length);
                        }
                        break;
                    }

                    default:
                        break;
                }

                var httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
                return MakeResponse(httpWebResponse, requestInfo);
            }
            catch(WebException exWeb)
            {
                Response res = null;
                HttpWebResponse webResponse = (HttpWebResponse) exWeb.Response;
                if(webResponse != null)
                    res = MakeResponse((HttpWebResponse) exWeb.Response, requestInfo);

                if(requestInfo.ThrowRestExceptionWhenStatusNotOK)
                    throw new RESTException(exWeb.Status, exWeb.Message, exWeb, res);
                else
                    return res;
            }
            catch(Exception ex)
            {
                throw new RESTException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }

        private static Response MakeResponse(HttpWebResponse httpWebResponse, RequestInfo requestInfo)
        {
            using(httpWebResponse)
            {
                Response res = new Response() {
                    StatusCode = httpWebResponse.StatusCode,
                    Headers = httpWebResponse.Headers.AllKeys
                            .Select(key => new {
                                Key = key,
                                Value = httpWebResponse.Headers[key]
                            })
                            .ToDictionary(pKey => pKey.Key, pValue => pValue.Value),
                    Encoding = (httpWebResponse.ContentEncoding != string.Empty) ? Encoding.GetEncoding(httpWebResponse.ContentEncoding) : requestInfo.Encoding,
                    ResponseDataType = MediaTypeExtension.GetMediaType(httpWebResponse.ContentType)
                };


                using(Stream sr = httpWebResponse.GetResponseStream())
                {
                    byte[] body = new byte[httpWebResponse.ContentLength > 0 ? httpWebResponse.ContentLength : 0];
                    res.Body = new byte[body.Length];

                    if(body.Length > 0)
                    {
                        sr.Read(body, offset: 0, count: body.Length);
                        Array.Copy(body, res.Body, body.Length);
                    }
                }

                return res;
            }
        }
    }
}
