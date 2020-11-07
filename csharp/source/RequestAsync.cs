using RESTClient.Enum;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RESTClient
{
    public static class RequestAsync
    {
        public static async Task<T> CallWhenJsonResponse<T>(RequestInfo requestInfo)
        {
            var response = await Call(requestInfo);
            return response.DeserializeBody<T>();
        }

        public static async Task<Response> Call(RequestInfo requestInfo)
        {
            try
            {
                Response res = new Response();
                using(var httpWebResponse = await GetHttpWebResponse(requestInfo))
                {
                    res.StatusCode = httpWebResponse.StatusCode;
                    res.Headers = httpWebResponse.Headers.AllKeys
                        .Select(key => new KeyValuePair<string, string>(key, httpWebResponse.Headers[key]))
                        .ToList();
                    res.Encoding = (httpWebResponse.ContentEncoding != string.Empty) ? Encoding.GetEncoding(httpWebResponse.ContentEncoding) : requestInfo.Encoding;
                    res.ResponseDataType = MediaTypeExtension.GetMediaType(httpWebResponse.ContentType);

                    using(Stream sr = httpWebResponse.GetResponseStream())
                    {
                        res.Body = new byte[httpWebResponse.ContentLength];
                        byte[] body = new byte[res.Body.Length];

                        if(body.Length > 0)
                        {
                            sr.Read(res.Body, offset: 0, count: body.Length);
                        }
                    }
                }

                return res;
            }
            catch(Exception ex) when(!(ex is RestException))
            {
                throw new RestException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }

        private static async Task<HttpWebResponse> GetHttpWebResponse(RequestInfo requestInfo)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(requestInfo.GetURI());
                webRequest.Method = requestInfo.Method.GetName();
                webRequest.ContentType = requestInfo.RequestDataType.GetContentType();

                foreach(var keyValue in requestInfo.GetHeader())
                {
                    webRequest.Headers.Add(keyValue.Key, keyValue.Value);
                }

                switch(requestInfo.Method)
                {
                    case HttpMethod.PATCH:
                    case HttpMethod.POST:
                    case HttpMethod.PUT:
                    {
                        using(var requestStream = await webRequest.GetRequestStreamAsync())
                        {
                            byte[] buffer = requestInfo.GetBodyBytes();
                            requestStream.Write(buffer, buffer.Length, buffer.Count());

                            webRequest.ContentLength = buffer.Count();
                        }
                        break;
                    }

                    default:
                        break;
                }

                return await webRequest.GetResponseAsync() as HttpWebResponse;
            }
            catch(WebException exWeb)
            {
                Response res = new Response();
                using(var httpWebResponse = (HttpWebResponse) exWeb.Response)
                {
                    res.StatusCode = httpWebResponse.StatusCode;
                    res.Headers = httpWebResponse.Headers.AllKeys
                        .Select(key => new KeyValuePair<string, string>(key, httpWebResponse.Headers[key]))
                        .ToList();
                    res.Encoding = (httpWebResponse.ContentEncoding != string.Empty) ? Encoding.GetEncoding(httpWebResponse.ContentEncoding) : requestInfo.Encoding;
                    res.ResponseDataType = MediaTypeExtension.GetMediaType(httpWebResponse.ContentType);

                    using(Stream sr = httpWebResponse.GetResponseStream())
                    {
                        byte[] body = new byte[httpWebResponse.ContentLength];
                        sr.Read(body, offset: 0, count: body.Length);
                        res.Body = new byte[body.Length];

                        Array.Copy(body, res.Body, body.Length);
                    }
                }

                throw new RestException(exWeb.Status, exWeb.Message, exWeb, res);
            }
            catch(Exception ex)
            {
                throw new RestException(WebExceptionStatus.UnknownError, ex.Message, ex);
            }
        }
    }
}

