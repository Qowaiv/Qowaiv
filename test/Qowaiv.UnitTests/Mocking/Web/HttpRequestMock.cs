using System.Web;

namespace Qowaiv.UnitTests.Mocking.Web
{
	/// <summary>Represents a mocked HTTP-request.</summary>
	public class HttpRequestMock : HttpRequestBase
	{
		/// <summary>Gets and set the content-type.</summary>
		public override string ContentType
		{
			get { return m_ContentType; }
			set { m_ContentType = value; }
		}
		private string m_ContentType;
	}
}
