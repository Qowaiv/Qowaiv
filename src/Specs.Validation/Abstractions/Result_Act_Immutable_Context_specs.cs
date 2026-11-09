using Qowaiv.Validation.Abstractions;

namespace Abstractions.Result_Act_Immutable_Context_specs;

public class Valid_Model
{
    [Test]
    public void Act_on_success_is_executed_and_result_stays_valid()
        => Context.Valid.Act(Actions.Success, Context.Update)
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public void Act_on_success_null_value_is_executed_and_result_stays_valid()
        => Context.Valid.Act(Actions.SuccessNullValue, Context.Update)
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public void Act_on_failing_makes_result_invalid()
        => Context.Valid.Act(Actions.Failure, Context.Update)
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("Failure"));

    [Test]
    public void Act_with_update_returning_null_throws()
        => Context.Valid.Invoking(c => c.Act(Actions.Success, (_, _) => null!))
        .Should().Throw<NoValue>();

    [Test]
    public async Task ActAsync_on_success_is_executed_and_result_stays_valid()
        => (await Context.Valid.ActAsync(Actions.SuccessAsync, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_success_null_value_is_executed_and_result_stays_valid()
        => (await Context.Valid.ActAsync(Actions.SuccessNullValueAsync, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_failing_makes_result_invalid()
        => (await Context.Valid.ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("FailureAsync"));

    [Test]
    public async Task ActAsync_with_update_returning_null_throws()
    {
        var act = async () => await Context.Valid.ActAsync(Actions.SuccessAsync, (_, _) => null!);
        await act.Should().ThrowAsync<NoValue>();
    }
}

public class Task_Valid_Model
{
    [Test]
    public async Task ActAsync_on_sync_success_is_executed_and_result_stays_valid()
        => (await Context.Valid.Async().ActAsync(Actions.Success, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_sync_success_null_value_is_executed_and_result_stays_valid()
        => (await Context.Valid.Async().ActAsync(Actions.SuccessNullValue, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_async_success_is_executed_and_result_stays_valid()
        => (await Context.Valid.Async().ActAsync(Actions.SuccessAsync, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_async_success_null_value_is_executed_and_result_stays_valid()
        => (await Context.Valid.Async().ActAsync(Actions.SuccessNullValueAsync, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_sync_failing_makes_result_invalid()
        => (await Context.Valid.Async().ActAsync(Actions.Failure, Context.Update))
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("Failure"));

    [Test]
    public async Task ActAsync_on_async_failing_makes_result_invalid()
        => (await Context.Valid.Async().ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("FailureAsync"));
}

public class Invalid_Model
{
    [Test]
    public void Act_is_never_triggered()
        => Context.Invalid.Act(Actions.Failure, Context.Update)
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("InvalidContext"));

    [Test]
    public async Task ActAsync_is_never_triggered()
        => (await Context.Invalid.ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("InvalidContext"));
}

public class Task_Invalid_Model
{
    [Test]
    public async Task ActAsync_with_sync_is_never_triggered()
        => (await Context.Invalid.Async().ActAsync(Actions.Failure, Context.Update))
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("InvalidContext"));

    [Test]
    public async Task ActAsync_with_async_is_never_triggered()
        => (await Context.Invalid.Async().ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid().WithMessage(ValidationMessage.Error("InvalidContext"));
}

internal record Context(string Value)
{
    public static Result<Context> Valid => Result.For(new Context((string)null!));
    public static Result<Context> Invalid => Result.WithMessages<Context>(ValidationMessage.Error("InvalidContext"));

    public bool Updated { get; private set; }

    public static Context Update(Context _, string value) => new Context(value) with { Updated = true };
}

internal static class Actions
{
    public static Result<string> Success(Context _)
        => Result.For(nameof(Success));

    public static AsyncResult_string SuccessAsync(Context _)
        => Result.For(nameof(SuccessAsync)).Async();

    public static Result<string> SuccessNullValue(Context _)
        => Result.Null<string>();

    public static AsyncResult_string SuccessNullValueAsync(Context _)
        => Result.Null<string>().Async();

    public static Result<string> Failure(Context _)
        => Result.WithMessages<string>(ValidationMessage.Error(nameof(Failure)));

    public static AsyncResult_string FailureAsync(Context _)
        => Result.WithMessages<string>(ValidationMessage.Error(nameof(FailureAsync))).Async();
}
