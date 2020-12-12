using RESTClient.Enums;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RESTClient
{
    public class RequestBase
    {
        private readonly int MILLISECOND_ONE_SECOND = 1000;

        protected HttpWebRequest GetHttpWebRequest(RequestInfo requestInfo)
        {
            var httpWebRequest = MakeHttpWebRequest(requestInfo);
            if(requestInfo.Method == HttpMethod.Patch || requestInfo.Method == HttpMethod.Post
                || requestInfo.Method == HttpMethod.Put)
            {
                byte[] buffer = requestInfo.GetBodyBytes();
                httpWebRequest.ContentLength = buffer.Length;

                using var requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(buffer.AsSpan(0, buffer.Length));
            }

            return httpWebRequest;
        }

        protected async Task<HttpWebRequest> GetHttpWebRequestAsync(RequestInfo requestInfo)
        {
            var httpWebRequest = MakeHttpWebRequest(requestInfo);
            if(requestInfo.Method == HttpMethod.Patch || requestInfo.Method == HttpMethod.Post
                || requestInfo.Method == HttpMethod.Put)
            {
                byte[] buffer = requestInfo.GetBodyBytes();
                httpWebRequest.ContentLength = buffer.Length;

                using var requestStream = httpWebRequest.GetRequestStream();
                await requestStream.WriteAsync(buffer.AsMemory(0, buffer.Length));
            }

            return httpWebRequest;
        }

        private HttpWebRequest MakeHttpWebRequest(RequestInfo requestInfo)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestInfo.GetURI());
            httpWebRequest.Method = requestInfo.Method.ToString();
            httpWebRequest.ContentType = requestInfo.RequestMediaType.GetContentType();
            httpWebRequest.Timeout = requestInfo.TimeoutSecond * MILLISECOND_ONE_SECOND;
            httpWebRequest.ContinueTimeout = requestInfo.ContinueTimeoutSeconds * MILLISECOND_ONE_SECOND;
            httpWebRequest.KeepAlive = requestInfo.KeepAlive;

            if(requestInfo.Proxy != null)
                httpWebRequest.Proxy = requestInfo.Proxy;

            foreach(var keyValue in requestInfo.GetHeader())
            {
                httpWebRequest.Headers.Add(keyValue.Key, keyValue.Value);
            }

            return httpWebRequest;
        }

        protected Response MakeResponse(HttpWebResponse httpWebResponse, Encoding httpRequestEncoding)
        {
            using(httpWebResponse)
            {
                Response res = InitResponse(httpWebResponse, httpRequestEncoding);
                using(Stream httpResponseStream = httpWebResponse.GetResponseStream())
                {
                    byte[] body = new byte[httpWebResponse.ContentLength > 0 ? httpWebResponse.ContentLength : 0];
                    res.Body = new byte[body.Length];

                    if(body.Length > 0)
                    {
                        httpResponseStream.Read(body, offset: 0, count: body.Length);
                        Array.Copy(body, res.Body, body.Length);
                    }
                }

                return res;
            }
        }

        protected async Task<Response> MakeResponseAsync(HttpWebResponse httpWebResponse, Encoding httpRequestEncoding)
        {
            using(httpWebResponse)
            {
                Response res = InitResponse(httpWebResponse, httpRequestEncoding);
                using(Stream httpResponseStream = httpWebResponse.GetResponseStream())
                {
                    byte[] body = new byte[httpWebResponse.ContentLength > 0 ? httpWebResponse.ContentLength : 0];
                    res.Body = new byte[body.Length];

                    if(body.Length > 0)
                    {
                        await httpResponseStream.ReadAsync(body.AsMemory(start: 0, length: body.Length));
                        Array.Copy(body, res.Body, body.Length);
                    }
                }

                return res;
            }
        }

        private static Response InitResponse(HttpWebResponse httpWebResponse, Encoding httpRequestEncoding) => new Response() {
            StatusCode = httpWebResponse.StatusCode,
            Headers = httpWebResponse.Headers.AllKeys
                    .Select(key => new {
                        Key = key,
                        Value = httpWebResponse.Headers[key]
                    })
                    .ToDictionary(pKey => pKey.Value, pValue => pValue.Value),
            Encoding = string.IsNullOrEmpty(httpWebResponse.ContentEncoding) ? httpRequestEncoding : Encoding.GetEncoding(httpWebResponse.ContentEncoding),
            ResponseDataType = MediaTypeExtension.GetMediaType(httpWebResponse.ContentType)
        };
    }
}