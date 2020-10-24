using System.Net;

namespace RESTClient
{
    public static class Request
    {
        public static T Call<T>(RequestInfo requestInfo)
        {
            return default;
        }

        public static void Call(RequestInfo requestInfo)
        {
        }

        /*
         * TODO: async method
        public static async T CallAsync<T>(RequestInfo requestInfo)
        {
            return default;
        }
        */

        private static void GetHttpWebRequest(RequestInfo requestInfo)
        {
            HttpWebRequest webRequest = (HttpWebRequest) HttpWebRequest.Create(requestInfo.GetURI());
        }
    }
}
