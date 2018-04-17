using System.Web;

namespace Qowaiv.Web.UnitTests.Mocking
{
    /// <summary>Represents a mocked HTTP-request.</summary>
    public class HttpRequestMock : HttpRequestBase
    {
        /// <summary>Gets and set the content-type.</summary>
        public override string ContentType { get; set; }
    }
}
