namespace RESTClient
{
    public enum HttpMethod
    {
        GET,
        HEAD,
        POST,
        PUT,
        PATCH,
        DELETE
    }

    internal static class HttpMethodExtensions
    {
        internal static string GetName(this HttpMethod httpMethod) => httpMethod.ToString().ToLower();
    }
}
