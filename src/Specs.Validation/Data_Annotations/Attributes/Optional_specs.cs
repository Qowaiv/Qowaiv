using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Optional_specs;

public class Is_valid_for
{
    [Test]
    [MethodDataSource(nameof(Models))]
    public void Not_empty_collection(object model)
       => new OptionalAttribute().IsValid(model).Should().BeTrue();

    public static IEnumerable<object?> Models() =>
    [
        null,
        string.Empty,
        Array.Empty<int>(),
        new[] { 42 },
        "some string",
        new object(),
    ];
}
