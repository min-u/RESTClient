using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RESTClient
{
    public static class JsonUtil
    {
        private static JsonSerializer ignoreMissingMemberJsonSeralizer = new JsonSerializer() {
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };

        public static T ToObject<T>(this JToken jtoken, bool ignoreMissingMember = false)
        {
            if(ignoreMissingMember)
                return jtoken.ToObject<T>(ignoreMissingMemberJsonSeralizer);
            else
                return jtoken.ToObject<T>();
        }
    }
}
