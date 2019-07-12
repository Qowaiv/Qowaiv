namespace Qowaiv.Validation.DataAnnotations.UnitTests.Models
{
    public class ModelWithForbiddenValues
    {
        [ForbiddenValues("spam@qowaiv.org")]
        public EmailAddress Email { get; set; }
    }
}
