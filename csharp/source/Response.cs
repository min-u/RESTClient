using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace RESTClient
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; internal set; }

        public byte[] Body { get; internal set; }

        public Encoding Encoding { get; internal set; }

        public List<KeyValuePair<string, string>> Headers { get; internal set; }

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
                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(this.GetBodyString());

                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeXmlNode(xmldoc));
            }
            else
            {
                throw new NotSupportedException("not support Response#DeserializeBody: ResponseDataType not in ('JSON', 'XML')");
            } 
        }
    }
}
