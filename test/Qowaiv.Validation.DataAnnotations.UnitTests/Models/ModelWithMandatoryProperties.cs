using System.ComponentModel.DataAnnotations;

namespace Qowaiv.Validation.DataAnnotations.UnitTests.Models
{
    public class ModelWithMandatoryProperties
    {
        [Mandatory, Display(Name = "E-mail address")]
        public EmailAddress Email { get; set; }

        [Mandatory]
        public string SomeString { get; set; }
    }
}
