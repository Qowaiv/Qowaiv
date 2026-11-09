using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Optional_specs;

public class Is_valid_for
{
    [Arguments(nameof(Models))]
    public void Not_empty_collection(object model)
       => new OptionalAttribute().IsValid(model).Should().BeTrue();

    private static IEnumerable<object?> Models()
    {
        yield return null;
        yield return string.Empty;
        yield return Array.Empty<int>();
        yield return new[] { 42 };
        yield return "some string";
        yield return new object();
    }
}
