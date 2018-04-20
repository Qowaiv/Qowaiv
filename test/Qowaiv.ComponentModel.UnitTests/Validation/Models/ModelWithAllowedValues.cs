using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.Globalization;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
    public class ModelWithAllowedValues
    {
        [AllowedValues("", "BE", "NL", "LU")]
        public Country Country { get; set; }
    }
}
