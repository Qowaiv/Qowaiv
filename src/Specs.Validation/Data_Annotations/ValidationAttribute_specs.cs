using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Data_Annotations.ValidationAttribute_specs;

public class All
{
    public static IEnumerable<Type> Attributes => typeof(ValidationMessage).Assembly
        .GetExportedTypes()
        .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(ValidationAttribute)));

    [Test]
    [MethodDataSource(nameof(Attributes))]
    public void are_decorated_with_ValidatesAttribute(Type attribute)
        => attribute.Should().BeDecoratedWith<ValidatesAttribute>();
}
