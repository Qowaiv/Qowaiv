using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Items_specs;

public class Validates
{
    [Test]
    public void items_according_to_wrapped_attribute()
    {
        var model = new Data_Annotations.Model.With.ValidatableArrayItems 
        {
            Numbers = [0, 42, 13],
            Emails = [EmailAddress.Parse("info@qowaiv.org"), EmailAddress.Unknown],
        };
        model.ValidateAnnotations()
            .Should().BeInvalid()
            .WithMessages(
                ValidationMessage.Error("The value of the Number field is not allowed.", "Numbers[0]"),
                ValidationMessage.Error("The value of the Number field is not allowed.", "Numbers[2]"),
                ValidationMessage.Error("Emails must be specified.", "Emails[1]"));
    }
}
