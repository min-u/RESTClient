using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using RestClient.Net5.Enums;

using Newtonsoft.Json;

namespace RestClient.Net5
{
    public class RequestInfo
    {
        public string URI { private get; set; } = string.Empty;

        public HttpMethod Method { internal get; set; } = HttpMethod.Get;

        public MediaType RequestMediaType { internal get; set; } = MediaType.JSON;

        public dynamic Query { private get; set; }

        public dynamic Body { private get; set; }

        public dynamic Headers { private get; set; }

        public Encoding Encoding { internal get; set; } = Encoding.UTF8;

        public bool ThrowRestExceptionWhenStatusNotOK { get; set; } = false;

        public bool KeepAlive { get; set; } = true;

        public int TimeoutSecond { get; set; } = 1;

        public int ContinueTimeoutSeconds { get; set; } = 1;

        public WebProxy Proxy { get; set; } = null;

        internal string GetURI() => $"{this.URI}{this.GetQuery()}";

        private string GetQuery()
        {
            string query = this.GetKeyValueString(this.Query);
            return $"{(query != string.Empty ? "?" : string.Empty)}{query}";
        }

        internal byte[] GetBodyBytes()
        {
            string body = string.Empty;
            if(this.Body != null)
            {
                switch(this.RequestMediaType)
                {
                    case MediaType.JSON:
                        body = JsonConvert.SerializeObject(this.Body);
                        break;

                    default:
                        body = GetKeyValueString(this.Body);
                        break;
                }
            }

            return this.Encoding.GetBytes(body);
        }

        internal List<KeyValuePair<string, string>> GetHeader() => this.GetKeyValuePairs(this.Headers);

        private List<KeyValuePair<string, string>> GetKeyValuePairs(dynamic obj)
        {
            List<KeyValuePair<string, string>> keyValuePairs = null;
            if (obj != null)
            {
                Type type = obj.GetType();
                keyValuePairs = type.GetFields()
                    .Select(field => new KeyValuePair<string, string>(field.Name, WebUtility.UrlEncode(field.GetValue(obj))))
                    .ToList();
            }

            return keyValuePairs ?? new List<KeyValuePair<string, string>>();
        }

        private string GetKeyValueString(dynamic obj)
        {
            List<KeyValuePair<string, string>> keyValuePairs = GetKeyValuePairs(obj);
            return string.Join("&", keyValuePairs.Select(keyValue => $"{keyValue.Key}={keyValue.Value}"));
        }
    }
}