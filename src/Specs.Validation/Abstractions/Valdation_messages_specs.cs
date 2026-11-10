using Qowaiv.Validation.Abstractions;
using static Specs.Abstractions.Arrange;

namespace Abstractions.Valdation_messages_specs;

public class Messages
{
    [Test]
    public void Are_sorted()
        => ValidationMessages.New([Warning1, Info1, Error1, Info2])
        .Should().BeEquivalentTo([Error1, Warning1, Info1, Info2]);

    [Test]
    public void with_Append_are_sorted_and_leaves_original_unchanged()
    {
        var messages = ValidationMessages.New([Warning1, Info1, Error1, Info2]);
        var addition = messages.Append([Warning2, Error2]);

        messages.AsObject().Should().BeEquivalentTo(new
        {
            Count = 4,
            Errors = array(Error1),
            Warnings = array(Warning1),
            Infos = array(Info1, Info2),
        });

        addition.AsObject().Should().BeEquivalentTo(new
        {
            Count = 6,
            Errors = array(Error1, Error2),
            Warnings = array(Warning1, Warning2),
            Infos = array(Info1, Info2),
        });
    }

    [Test]
    public void Filters_Errors()
        => ValidationMessages.New([Warning1, Info1, Error1, Error2,Info2]).Errors
        .Should().BeEquivalentTo([Error1, Error2]);

    [Test]
    public void Filters_Warnings()
        => ValidationMessages.New([Warning1, Info1, Error1, Error2, Warning2, Info2]).Warnings
        .Should().BeEquivalentTo([Warning1, Warning2]);
    
    [Test]
    public void Filters_Infos()
        => ValidationMessages.New([Warning1, Info1, Error1, Error2, Warning2, Info2]).Infos
        .Should().BeEquivalentTo([Info1, Info2]);

    [Test]
    public void Ingores_null_args()
        => ValidationMessages.New([Warning1, null, Error1])
        .Should().BeEquivalentTo([Error1, Warning1]);

    [Test]
    public void Ingores_None_args()
        => ValidationMessages.New([Warning1, ValidationMessage.None, Error1])
        .Should().BeEquivalentTo([Error1, Warning1]);

    [Test]
    [Arguments((ValidationSeverity)(-2))]
    [Arguments((ValidationSeverity)(+4))]
    public void throws_on_invalid_severity(ValidationSeverity severity)
        => severity.Invoking(s => ValidationMessages.New([Warning1, null, InvalidSeverity(s), Error1])).Should().Throw<ArgumentOutOfRangeException>()
        .WithMessage("Severity * is not Error, Warning, Info, or None.*")
        .And.ParamName.Should().Be("messages[2]");
}

public class Not_initalized_does_not_throw
{
    private readonly ValidationMessages NotInitalized = default;

    [Test]
    public void Count()
        => NotInitalized.Count.Should().Be(0);

    [Test]
    public void GetEnumerator()
        => NotInitalized.GetEnumerator().MoveNext().Should().BeFalse();

    [Test]
    public void Errors()
        => NotInitalized.Errors.Should().BeEmpty();

    [Test]
    public void Warnings()
        => NotInitalized.Warnings.Should().BeEmpty();

    [Test]
    public void Infos()
        => NotInitalized.Infos.Should().BeEmpty();
}
