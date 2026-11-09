using Qowaiv.Validation.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Data_annotations.Attributes.Unique_values_specs;

public  class Does_not_allow
{
    [Test]
    public void Comparer_types_that_do_not_implement_IEqualityComparer()
    {
        Func<UniqueAttribute<Country>> ctor = () => new UniqueAttribute<Country>(typeof(string));
        ctor.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Comparer_types_that_do_not_implement_IEqualityComparer_T()
    {
        Func<UniqueAttribute<Country>> ctor = () => new UniqueAttribute<Country>(typeof(EqualityComparer));
        ctor.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Validation_on_non_IEnumerables()
    {
        Func<bool> validate = () => new UniqueAttribute<int>().IsValid(42);
        validate.Should().Throw<ArgumentException>();
    }
}

public class Is_valid_for
{
    [Test]
    public void Null()
        => new UniqueAttribute<int>().IsValid(null).Should().BeTrue();

    [Test]
    public void Empty_collection()
       => new UniqueAttribute<int>().IsValid(Array.Empty<int>()).Should().BeTrue();

    [Test]
    public void distinct_list_according_to_default_comparer()
        => new UniqueAttribute<int>().IsValid(new[] { 42, 69 }).Should().BeTrue();

    [Test]
    public void distinct_list_according_to_IEqualityComparer_of_object()
        => new UniqueAttribute<int>(typeof(EqualityComparer_of_int)).IsValid(new[] { 42, 69 }).Should().BeTrue();
}

public class Is_not_valid_for
{
    [Test]
    public void nondistinct_list_according_to_default_comparer()
        => new UniqueAttribute<int>().IsValid(new[] { 42, 69, 69 }).Should().BeFalse();

    [Test]
    public void nondistinct_list_according_to_IEqualityComparer_of_object()
        => new UniqueAttribute<int>(typeof(EqualityComparer_of_int)).IsValid(new[] { 42, 69, 69 }).Should().BeFalse();
}

public class With_message
{
    [Test]
    [Arguments("nl", "Alle waarden van het veld Values zouden verschillend moeten zijn.")]
    [Arguments("en", "All values of the Values field should be unique.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model().ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Values"));
    }
    internal class Model
    {
        [Unique<int>]
        public int[] Values { get; init; } = [17, 17];
    }
}


internal class EqualityComparer : IEqualityComparer
{
    public new bool Equals(object? x, object? y) 
        => object.Equals(x, y)
        || (x is int x_ && y is int y_ && x_ == y_);

    public int GetHashCode(object obj) 
        => obj is int num ? num : RuntimeHelpers.GetHashCode(obj);
}


internal class EqualityComparer_of_int : IEqualityComparer<int>
{
    public bool Equals(int x, int y) => x == y;

    public int GetHashCode(int obj) => obj;
}
