using Qowaiv.Validation.Abstractions;
using System.Xml.Linq;
using static Specs.Abstractions.Arrange;

namespace Abstractions.Result_specs;

public class Valid_result
{
    [Test]
    public void Contains_no_ErrorMessages()
    {
        var result = Result.WithMessages(Warning1, Info1, Info2);
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Has_access_to_Value()
    {
        var result = Result.For(2);
        result.Value.Should().Be(2);
    }
}

public class Invalid_result
{
    [Test]
    public void Contains_at_least_one_ErrorMessage()
    {
        var result = Result.WithMessages(TestMessages.AsEnumerable());
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public void Has_no_access_to_Value()
    {
        var result = Result.For(new object(), ValidationMessage.Error("Not OK"));
        result.Invoking(r => r.Value)
            .Should().Throw<InvalidModel>();
    }
}

public class Null_result
{
    [Test]
    public void valid_if_explicit() => Result.Null<object>().IsValid.Should().BeTrue();

    [Test]
    public void valid_for_nullable_struct() => Result.Null<int?>().IsValid.Should().BeTrue();

    [Test]
    public void valid_if_explicit_with_messages()
    {
        var messages = new List<IValidationMessage> { ValidationMessage.Warn("Some warning") };
        Result.Null<object>(messages).IsValid.Should().BeTrue();
    }

    [Test]
    public void invalid_if_implicit()
    {
        Func<object> for_Null = () => Result.For<object>(null!);
        for_Null.Should().Throw<ArgumentNullException>()
            .WithMessage(("The value of the Result<Object> can not be null. (Parameter 'Value')"));
    }

    [Test]
    public void invalid_if_implicit_with_messages()
        => new object()
            .Invoking(_ => Result.WithMessages<object>())
            .Should().Throw<NoValue>();

    [Test]
    public void invalid_if_implicit_with_empty_messages()
        => Array.Empty<IValidationMessage>()
            .Invoking(Result.WithMessages<object>)
            .Should().Throw<NoValue>();
}

public class Filtering
{
    [Test]
    public void Error_messages_is_done_via_the_Errors_property()
    {
        var result = Result.WithMessages(TestMessages);
        result.Errors.Should().BeEquivalentTo([Error1, Error2]);
    }

    [Test]
    public void Warning_messages_is_done_via_the_Warnings_property()
    {
        var result = Result.WithMessages(TestMessages);
        result.Warnings.Should().BeEquivalentTo([Warning1, Warning2]);
    }

    [Test]
    public void Info_messages_is_done_via_the_Infos_property()
    {
        var result = Result.WithMessages(TestMessages);
        result.Infos.Should().BeEquivalentTo([Info1, Info2]);
    }
}

public class Casting
{
    [Test]
    public void Implicit_from_T_to_Result_of_T_is_supported()
    {
        Result<int> result = 17;
        result.Should().BeValid().And.Value.Should().Be(17);
    }

    [Test]
    public void Explicit_from_Result_of_T_to_T_is_supported()
    {
        var result = Result.For(666);
        result.Should().BeValid().And.Value.Should().Be(666);
    }

    public class With_method
    {
        [Test]
        public void Result_of_valid_with_subtype_of_T_is_supported()
        {
            var value = XDocument.Parse("<root />");
            Result.For(value).Cast<XNode>().Should().BeValid()
                .And.Value.Should().BeSameAs(value);
        }

        [Test]
        public void Null_value_with_subtype_of_T_is_supported()
            => Result.Null<XDocument>().Cast<XNode>().Should().BeValid()
            .And.Value.Should().BeNull();

        [Test]
        public void To_Result_of_invalid_with_subtype_of_T_is_supported()
            => Result.WithMessages<XDocument>(Error1).Cast<XNode>().Should().BeInvalid();

        [Test]
        public void Is_not_supported_TOut_not_being_subtype_of_TModel()
            => Result.For(42).Invoking(r => r.Cast<long>())
                .Should().Throw<InvalidCastException>()
                .WithMessage("Unable to cast object of type 'Result<System.Int32>' to type 'Result<System.Int64>'.");

        [Test]
        public void Can_always_cast_on_error()
            => Result.For(42, Error1).Cast<string>().Should().BeInvalid();
    }
}

public class ThrowIfInvalid
{
    [Test]
    public void Nothing_when_valid()
        => 17.Invoking(v => Result.For(v)).Should().NotThrow();

    [Test]
    public void InvalidModel_when_invalid_model()
    {
        var result = Result.WithMessages<int>(ValidationMessage.Error("Oops"));
        result.Invoking(_ => result.ThrowIfInvalid())
            .Should().Throw<InvalidModel>();
    }

    [Test]
    public void InvalidModel_when_invalid_result()
    {
        var result = Result.WithMessages(ValidationMessage.Error("Oops"));
        result.Invoking(_ => result.ThrowIfInvalid())
            .Should().Throw<InvalidModel>();
    }

    [Test]
    public void NoModel_when_not_provided() 
        => Warning1.Invoking(m => Result.WithMessages<int>(m))
        .Should().Throw<NoValue>();
}
