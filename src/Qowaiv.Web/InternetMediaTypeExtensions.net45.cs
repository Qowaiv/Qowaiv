using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Qowaiv.Web
{
    /// <summary>Extensions that result in retrieving a <see cref="InternetMediaType"/>.</summary>
    public static class InternetMediaTypeExtensions
    {
        /// <summary>Gets the content type.</summary>
        [ExcludeFromCodeCoverage]
        public static InternetMediaType GetContentType(this HttpRequest request)
        {
            Guard.NotNull(request, nameof(request));
            return new HttpRequestWrapper(request).GetContentType();
        }
        /// <summary>Sets the content type.</summary>
        [ExcludeFromCodeCoverage]
        public static void SetContentType(this HttpRequest request, InternetMediaType mediaType)
        {
            Guard.NotNull(request, nameof(request));
            new HttpRequestWrapper(request).SetContentType(mediaType);
        }

        /// <summary>Gets the content type.</summary>
        [ExcludeFromCodeCoverage]
        public static InternetMediaType GetContentType(this HttpResponse response)
        {
            Guard.NotNull(response, nameof(response));
            return new HttpResponseWrapper(response).GetContentType();
        }
        /// <summary>Sets the content type.</summary>
        [ExcludeFromCodeCoverage]
        public static void SetContentType(this HttpResponse response, InternetMediaType mediaType)
        {
            Guard.NotNull(response, nameof(response));
            new HttpResponseWrapper(response).SetContentType(mediaType);
        }

        /// <summary>Gets the content type.</summary>
        public static InternetMediaType GetContentType(this HttpRequestBase request)
        {
            Guard.NotNull(request, nameof(request));
            return InternetMediaType.Parse(request.ContentType);
        }
        /// <summary>Sets the content type.</summary>
        public static void SetContentType(this HttpRequestBase request, InternetMediaType mediaType)
        {
            Guard.NotNull(request, nameof(request));
            request.ContentType = mediaType.ToString();
        }

        /// <summary>Gets the content type.</summary>
        public static InternetMediaType GetContentType(this HttpResponseBase response)
        {
            Guard.NotNull(response, nameof(response));
            return InternetMediaType.Parse(response.ContentType);
        }
        /// <summary>Sets the content type.</summary>
        public static void SetContentType(this HttpResponseBase response, InternetMediaType mediaType)
        {
            Guard.NotNull(response, nameof(response));
            response.ContentType = mediaType.ToString();
        }
    }
}
