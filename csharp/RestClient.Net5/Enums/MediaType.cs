namespace RestClient.Net5.Enums
{
    public enum MediaType
    {
        JSON,
        TEXT,
        FORM
    }

    public static class MediaTypeExtension
    {
        private const string CONTENT_TYPE_JSON = "application/json";
        private const string CONTENT_TYPE_FORM = "application/x-www-form-urlencoded";
        private const string CONTENT_TYPE_XML = "text/xml";
        private const string CONTENT_TYPE_TEXT = "text/plan";

        internal static string GetContentType(this MediaType mediaType)
        {
            switch(mediaType)
            {
                case MediaType.JSON:
                    return CONTENT_TYPE_JSON;

                case MediaType.FORM:
                    return CONTENT_TYPE_FORM;

                default:
                    return CONTENT_TYPE_TEXT;
            }
        }

        internal static MediaType GetMediaType(string contentType)
        {
            if(contentType == CONTENT_TYPE_JSON)
            {
                return MediaType.JSON;
            }
            else if(contentType == CONTENT_TYPE_FORM)
            {
                return MediaType.FORM;
            }
            else
            {
                return MediaType.TEXT;
            }
        }
    }
}