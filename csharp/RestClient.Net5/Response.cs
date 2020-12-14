using Newtonsoft.Json;

using RESTClient.Enums;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RESTClient
{
    public class Response
    {
        private static JsonSerializerSettings ignoreMissingMemberJsonSerializeSettings = new JsonSerializerSettings() {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public HttpStatusCode StatusCode { get; internal set; }

        public byte[] Body { get; internal set; }

        public Encoding Encoding { get; internal set; }

        public Dictionary<string, string> Headers { get; internal set; }

        public MediaType ResponseDataType { get; internal set; }

        public string GetBodyString() => Encoding.GetString(this.Body);

        public T DeserializeJson<T>(bool ignoreMissingMember = false)
        {
            if(ignoreMissingMember)
                return JsonConvert.DeserializeObject<T>(this.GetBodyString(), ignoreMissingMemberJsonSerializeSettings);
            else
                return JsonConvert.DeserializeObject<T>(this.GetBodyString());
        }
    }
}