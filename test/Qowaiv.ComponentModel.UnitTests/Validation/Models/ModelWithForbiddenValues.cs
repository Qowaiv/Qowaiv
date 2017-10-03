using Qowaiv.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
	public class ModelWithForbiddenValues
    {
		[ForbiddenValues("spam@qowaiv.org")]
		public EmailAddress Email { get; set; }
	}
}
