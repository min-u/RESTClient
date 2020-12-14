using Newtonsoft.Json;

using RESTClient.Enum;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace RESTClient
{
    public class RequestInfo
    {
        public string URI { private get; set; } = string.Empty;

        /// <summary>
        /// default GET
        /// </summary>
        public HttpMethod Method { internal get; set; } = HttpMethod.GET;

        /// <summary>
        /// default JSON
        /// </summary>
        public MediaType RequestDataType { internal get; set; } = MediaType.JSON;

        public dynamic Query { private get; set; }

        public dynamic Body { private get; set; }

        /// <summary>
        /// content-type and encoding do not need to be included.
        /// </summary>
        public dynamic Header { private get; set; }

        /// <summary>
        /// default UTF8
        /// </summary>
        public Encoding Encoding { internal get; set; } = Encoding.UTF8;

        public bool ThrowRestExceptionWhenStatusNotOK { get; set; } = false;

        public bool KeepAlive { get; set; } = true;

        public int TimeoutSecond { get; set; } = 1;

        public int ContinueTimeoutSeconds { get; set; } = 1;

        public WebProxy Proxy { internal get; set; } = default;

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
                switch(this.RequestDataType)
                {
                    case MediaType.JSON:
                        body = JsonConvert.SerializeObject(this.Body);
                        break;

                    case MediaType.XML:
                    {
                        XmlSerializer xml = new XmlSerializer(this.Body.GetType());
                        using(MemoryStream ms = new MemoryStream())
                        {
                            xml.Serialize(ms, this.Body);
                            using(StreamReader sr = new StreamReader(ms))
                            {
                                body = sr.ReadToEnd();
                            }
                        }
                        break;
                    }

                    default:
                        body = GetKeyValueString(this.Body);
                        break;
                }
            }

            return this.Encoding.GetBytes(body);
        }

        internal List<KeyValuePair<string, string>> GetHeader() => this.GetKeyValuePairs(this.Header);

        private List<KeyValuePair<string, string>> GetKeyValuePairs(dynamic obj)
        {
            List<KeyValuePair<string, string>> keyValuePairs = null;
            if(obj != null)
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
