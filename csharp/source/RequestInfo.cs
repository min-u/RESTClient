using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

        /// <summary>
        /// default JSON
        /// </summary>
        public MediaType ResponseDataType { internal get; set; } = MediaType.JSON;

        public dynamic Query { private get; set; }

        public dynamic Body { private get; set; }

        public dynamic Header { private get; set; }

        /// <summary>
        /// default UTF8
        /// </summary>
        public Encoding Encode { internal get; set; } = Encoding.UTF8;


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

                    default:
                        body = GetKeyValueString(this.Body);
                        break;
                }
            }

            return this.Encode.GetBytes(body);
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
