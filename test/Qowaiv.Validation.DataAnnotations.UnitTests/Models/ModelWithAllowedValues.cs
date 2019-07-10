using Qowaiv.Globalization;

namespace Qowaiv.Validation.DataAnnotations.UnitTests.Models
{
    public class ModelWithAllowedValues
    {
        [AllowedValues("", "BE", "NL", "LU")]
        public Country Country { get; set; }
    }
}
