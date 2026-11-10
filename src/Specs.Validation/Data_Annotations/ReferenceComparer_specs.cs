using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.ReferenceComparer_specs;

public class Equal_by_reference
{
    [Test]
    public void Two_instances_with_same_hash_are_diffrent()
    => ReferenceComparer.Instance.Equals(
        new SingleState(),
        new SingleState()).Should().BeFalse();

    [Test]
    public void Same_reference_are_equal()
    {
        var model = new SingleState();
        var other = model;

        ReferenceComparer.Instance
            .Equals(model, other)
            .Should().BeTrue();
    }
}

internal sealed class SingleState : IEquatable<SingleState>
{
    public override bool Equals(object? obj) => Equals(obj as SingleState);

    public bool Equals(SingleState? other) => other is { };

    public override int GetHashCode() => 17;
}
