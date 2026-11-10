using Qowaiv.Validation.Abstractions;

namespace Test_tools.Result_Should_specs;

public class BeValid
{
    [Test]
    public void fails_for_invalid_result()
    {
        var result = Result.WithMessages(ValidationMessage.Error("Broken"));
        Action assert = () => result.Should().BeValid("something went wrong");
        assert.Should().Throw<AssertionFailed>().WithMessage(
@"Actual Result is not valid because something went wrong:
- ERROR   Broken");
    }

    [Test]
    public void passes_for_OK_result()
        => Result.OK.Should().BeValid("something went wrong");
}

public class BeInvalid
{
    [Test]
    public void fails_for_OK_result()
    {
        Action assert = () => Result.OK.Should().BeInvalid("something went wrong");
        assert.Should().Throw<AssertionFailed>().WithMessage(@"Actual Result.OK is not invalid because something went wrong.");
    }

    [Test]
    public void passes_for_invalid_result()
    {
        var result = Result.WithMessages(ValidationMessage.Error("Broken"));
        result.Should().BeInvalid("something went wrong");
    }
}

public class WithoutMessage
{
    [Test]
    public void fails_for_any_message()
    {
        var result = Result.WithMessages(ValidationMessage.Warn("Almost broken"), ValidationMessage.Info("Just that you know.", "Data"));
        Action assert = () => result.Should().BeValid().WithoutMessages();
        assert.Should().Throw<AssertionFailed>().WithMessage(
@"Expected no messages, but found:
- WARNING Almost broken
- INFO    Just that you know. Prop: Data");
    }

    [Test]
    public void passes_for_no_messages()
        => Result.OK.Should().BeValid().WithoutMessages();
}

public class WithMessage
{
    [Test]
    public void fails_for_no_message()
    {
        Action assert = () => Result.OK.Should().BeValid().WithMessage(ValidationMessage.Info("Just that you know.", "Data"));
        assert.Should().Throw<AssertionFailed>().WithMessage("Expected a message, but found none.");
    }

    [Test]
    public void fails_for_different_message()
    {
        var result = Result.WithMessages(ValidationMessage.Warn("Almost broken"));
        Action assert = () => result.Should().BeValid().WithMessage(ValidationMessage.Info("Just that you know.", "Data"));
        assert.Should().Throw<AssertionFailed>().WithMessage(
@"Expected:
- INFO    Just that you know. Prop: Data
Actual:
- WARNING Almost broken");
    }

    [Test]
    public void fails_for_messages()
    {
        var result = Result.WithMessages(ValidationMessage.Warn("Almost broken"), ValidationMessage.Info("Just that you know.", "Data"));
        Action assert = () => result.Should().BeValid().WithMessage(ValidationMessage.Info("Just that you know.", "NoData"));
        assert.Should().Throw<AssertionFailed>().WithMessage(
@"Missing message:
- INFO    Just that you know. Prop: NoData
Extra messages:
- WARNING Almost broken
- INFO    Just that you know. Prop: Data");
    }

    [Test]
    public void passes_for_same_message()
        => Result.WithMessages(ValidationMessage.Warn("Almost broken"))
        .Should().BeValid().WithMessage(ValidationMessage.Warn("Almost broken"));
}

public class WithMessages
{
    [Test]
    public void fails_for_no_message()
    {
        Action assert = () => Result.OK.Should().BeValid().WithMessages(
            ValidationMessage.Warn("Almost broken"),
            ValidationMessage.Info("Just that you know.", "Data"));

        assert.Should().Throw<AssertionFailed>().WithMessage("Expected messages, but found none.");
    }

    [Test]
    public void fails_for_different_messages()
    {
        var result = Result.WithMessages(ValidationMessage.Warn("Almost broken"));
        Action assert = () => result.Should().BeValid().WithMessages(
            ValidationMessage.Warn("Almost broken"),
            ValidationMessage.Info("Just that you know.", "Data"));

        assert.Should().Throw<AssertionFailed>().WithMessage(
@"Missing message:
- INFO    Just that you know. Prop: Data");
    }

    [Test]
    public void passes_for_same_messages()
        => Result.WithMessages(
            ValidationMessage.Warn("Almost broken"),
            ValidationMessage.Info("Just that you know.", "Data"),
            ValidationMessage.Error("Broken"))
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("Broken"),
            ValidationMessage.Warn("Almost broken"),
            ValidationMessage.Info("Just that you know.", "Data"));
}

public class Value
{
    [Test]
    public void is_evaluated_for_a_valid_chain()
    {
        Action assert = () => Result.For(3).Should().BeValid().And.Value.Should().Be(13);
        assert.Should().Throw<Exception>().WithMessage("Expected * to be 13, but found 3 (difference of -10).");
    }

    [Test]
    public void is_evaluated_for_a_valid_chain_with_message_assert()
    {
        Action assert = () => Result.For(3).Should().BeValid().WithoutMessages().And.Value.Should().Be(13);
        assert.Should().Throw<Exception>().WithMessage("Expected * to be 13, but found 3 (difference of -10).");
    }

    [Test]
    public void is_not_evaluated_for_a_invalid_chain()
    {
        Action assert = () => Result.For(3).Should().BeValid().WithMessage(ValidationMessage.Info("Any")).And.Value.Should().Be(13);
        assert.Should().Throw<Exception>().WithMessage("Expected a message, but found none.");
    }
}

public class Null
{
    [Test]
    public void Should_BeValid_fails()
    {
        Result? result = null;
        Action assert = () => result.Should().BeValid();
        assert.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Result_Should_BeInvalid_fails()
    {
        Result? result = null;
        Action assert = () => result.Should().BeInvalid();
        assert.Should().Throw<ArgumentNullException>();
    }
}
