using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Qowaiv.Web
{
	public static class InternetMediaTypeExtensions
	{
		/// <summary>Gets the content type.</summary>
		[ExcludeFromCodeCoverage]
		public static InternetMediaType GetContentType(this HttpRequest request)
		{
			Guard.NotNull(request, "request");
			return new HttpRequestWrapper(request).GetContentType();
		}
		/// <summary>Sets the content type.</summary>
		[ExcludeFromCodeCoverage]
		public static void SetContentType(this HttpRequest request, InternetMediaType mediaType)
		{
			Guard.NotNull(request, "request");
			new HttpRequestWrapper(request).SetContentType(mediaType);
		}

		/// <summary>Gets the content type.</summary>
		[ExcludeFromCodeCoverage]
		public static InternetMediaType GetContentType(this HttpResponse reponse)
		{
			Guard.NotNull(reponse, "reponse");
			return new HttpResponseWrapper(reponse).GetContentType();
		}
		/// <summary>Sets the content type.</summary>
		[ExcludeFromCodeCoverage]
		public static void SetContentType(this HttpResponse reponse, InternetMediaType mediaType)
		{
			Guard.NotNull(reponse, "reponse");
			new HttpResponseWrapper(reponse).SetContentType(mediaType);
		}

		/// <summary>Gets the content type.</summary>
		public static InternetMediaType GetContentType(this HttpRequestBase request)
		{
			Guard.NotNull(request, "request");
			return InternetMediaType.Parse(request.ContentType);
		}
		/// <summary>Sets the content type.</summary>
		public static void SetContentType(this HttpRequestBase request, InternetMediaType mediaType)
		{
			Guard.NotNull(request, "request");
			request.ContentType = mediaType.ToString();
		}

		/// <summary>Gets the content type.</summary>
		public static InternetMediaType GetContentType(this HttpResponseBase reponse)
		{
			Guard.NotNull(reponse, "reponse");
			return InternetMediaType.Parse(reponse.ContentType);
		}
		/// <summary>Sets the content type.</summary>
		public static void SetContentType(this HttpResponseBase reponse, InternetMediaType mediaType)
		{
			Guard.NotNull(reponse, "reponse");
			reponse.ContentType = mediaType.ToString();
		}
	}
}
