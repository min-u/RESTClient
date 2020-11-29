using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestClient.Net5.Enums;

namespace RestClient.Net5
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; internal set; }

        public byte[] Body { get; internal set; }

        public Encoding Encoding { get; internal set; }

        public List<KeyValuePair<string, string>> Headers { get; internal set; }

        public MediaType ResponseDataType { get; internal set; }

        public string GetBodyString() => Encoding.GetString(this.Body);

        public T DeserializeBody<T>()
        {
            if(ResponseDataType == MediaType.JSON)
            {
                return JsonConvert.DeserializeObject<T>(this.GetBodyString());
            }
            else
            {
                throw new NotSupportedException("not support Response#DeserializeBody: ResponseDataType not in ('JSON')");
            }
        }
    }
}