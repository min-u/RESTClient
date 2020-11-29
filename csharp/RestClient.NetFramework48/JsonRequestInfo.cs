using RESTClient.Enum;

namespace RESTClient
{
    public class JsonRequestInfo: RequestInfo
    {
        public new MediaType RequestDataType { get; } = MediaType.JSON;
    }
}
