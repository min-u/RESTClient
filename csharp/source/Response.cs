using Newtonsoft.Json;

using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RESTClient
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; internal set; }

        public byte[] Body { get; internal set; }

        public Encoding Encoding { get; internal set; }

        public List<KeyValuePair<string, string>> Headers { get; internal set; }

        public T DeserializeBody<T>() => JsonConvert.DeserializeObject<T>(Encoding.GetString(this.Body));
    }
}
