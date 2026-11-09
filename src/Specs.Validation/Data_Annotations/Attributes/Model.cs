using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Data_Annotations;

internal static class Model
{
    public static class With
    {
        public sealed class ValidatableArrayItems
        {
            [Any]
            [Display(Name = "Number")]
            [Items<AllowedAttribute<int>>(42)]
            public int[] Numbers { get; init; } = [];

            [Items<MandatoryAttribute>(ErrorMessage = "Emails must be specified.")]
            public EmailAddress[] Emails { get; init; } = [];
        }
    }
}
