namespace RESTClient
{
    public class RequestResult
    {
        public Response Response { get; internal set; }
        public RESTException Exception { get; internal set; }
        public bool HasException { get => Exception != default; }
    }
}
