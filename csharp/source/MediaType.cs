namespace RESTClient
{
    public enum MediaType
    {
        JSON,
        TEXT,
        /// <summary>
        /// Used only for request.
        /// </summary>
        FORM,
        XML,
    }

    public static class ContentTypeExtension
    {
        internal static string GetContentType(this MediaType mediaType)
        {
            switch(mediaType)
            {
                case MediaType.JSON:
                    return "application/json";

                case MediaType.FORM:
                    return "application/x-www-form-urlencoded";

                case MediaType.XML:
                    return "text/xml";

                default:
                    return "text/plan";
            }
        }
    }
}
