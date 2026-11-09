using Qowaiv.Validation.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Data_annotations.Attributes.Distinct_values_specs;

public  class Does_not_allow
{
    [Test]
    public void Comparer_types_that_do_not_implement_IEqualityComparer()
    {
        Func<DistinctValuesAttribute> ctor = () => new DistinctValuesAttribute(typeof(string));
        ctor.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Validation_on_non_IEnumerables()
    {
        Func<bool> validate = () => new DistinctValuesAttribute().IsValid(42);
        validate.Should().Throw<ArgumentException>();
    }
}

public class Is_valid_for
{
    [Test]
    public void Null()
        => new DistinctValuesAttribute().IsValid(null).Should().BeTrue();

    [Test]
    public void Empty_collection()
       => new DistinctValuesAttribute().IsValid(Array.Empty<int>()).Should().BeTrue();

    [Test]
    public void distinct_list_according_to_default_comparer()
        => new DistinctValuesAttribute().IsValid(new[] { 42, 69 }).Should().BeTrue();

    [Test]
    public void distinct_list_according_to_IEqualityComparer()
        => new DistinctValuesAttribute(typeof(EqualityComparer)).IsValid(new[] { 42, 69 }).Should().BeTrue();

    [Test]
    public void distinct_list_according_to_IEqualityComparer_of_object()
        => new DistinctValuesAttribute(typeof(EqualityComparer_of_object)).IsValid(new[] { 42, 69 }).Should().BeTrue();
}

public class Is_not_valid_for
{
    [Test]
    public void nondistinct_list_according_to_default_comparer()
        => new DistinctValuesAttribute().IsValid(new[] { 42, 69, 69 }).Should().BeFalse();

    [Test]
    public void nondistinct_list_according_to_IEqualityComparer()
        => new DistinctValuesAttribute(typeof(EqualityComparer)).IsValid(new[] { 42, 69, 69 }).Should().BeFalse();

    [Test]
    public void nondistinct_list_according_to_IEqualityComparer_of_object()
        => new DistinctValuesAttribute(typeof(EqualityComparer_of_object)).IsValid(new[] { 42, 69, 69 }).Should().BeFalse();
}


internal class EqualityComparer : IEqualityComparer
{
    public new bool Equals(object? x, object? y) 
        => object.Equals(x, y)
        || (x is int x_ && y is int y_ && x_ == y_);

    public int GetHashCode(object obj) 
        => obj is int num ? num : RuntimeHelpers.GetHashCode(obj);
}


internal class EqualityComparer_of_object : IEqualityComparer<object>
{
    public new bool Equals(object? x, object? y) 
        => object.Equals(x, y)
        || (x is int x_ && y is int y_ && x_ == y_);

    public int GetHashCode(object obj)
        => obj is int num ? num : RuntimeHelpers.GetHashCode(obj);
}
