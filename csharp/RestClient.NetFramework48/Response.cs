using Newtonsoft.Json;

using RESTClient.Enum;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RESTClient
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; internal set; }

        public byte[] Body { get; internal set; }

        public Encoding Encoding { get; internal set; }

        public Dictionary<string, string> Headers { get; internal set; }

        internal MediaType ResponseDataType { private get; set; }

        public string GetBodyString() => Encoding.GetString(this.Body);

        public T DeserializeBody<T>()
        {
            if(ResponseDataType == MediaType.JSON)
            {
                return JsonConvert.DeserializeObject<T>(this.GetBodyString());
            }
            else if(ResponseDataType == MediaType.XML)
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    ms.Write(this.Body, 0, this.Body.Length);
                    ms.Position = 0;

                    XmlSerializer xml = new XmlSerializer(typeof(T));

                    return (T) xml.Deserialize(ms);
                }
            }
            else
            {
                throw new NotSupportedException("not support Response#DeserializeBody: ResponseDataType not in ('JSON', 'XML')");
            }
        }

        public async Task<T> DeserializeBodyAsync<T>()
        {
            if(ResponseDataType == MediaType.JSON)
            {
                return JsonConvert.DeserializeObject<T>(this.GetBodyString());
            }
            else if(ResponseDataType == MediaType.XML)
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    await ms.WriteAsync(this.Body, 0, this.Body.Length);
                    ms.Position = 0;

                    XmlSerializer xml = new XmlSerializer(typeof(T));

                    return (T) xml.Deserialize(ms);
                }
            }
            else
            {
                throw new NotSupportedException("not support Response#DeserializeBody: ResponseDataType not in ('JSON', 'XML')");
            }
        }
    }
}
