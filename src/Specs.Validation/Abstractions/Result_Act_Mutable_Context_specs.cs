using Qowaiv.Validation.Abstractions;

namespace Abstractions.Result_Act_Mutable_Context_specs;

public class Valid_Model
{
    [Test]
    public void Act_on_success_is_executed_and_result_stays_valid()
        => Context.Valid.Act(Actions.Success, Context.Update)
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public void Act_on_failing_makes_result_invalid()
        => Context.Valid.Act(Actions.Failure, Context.Update)
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("Failure"));

    [Test]
    public async Task ActAsync_on_success_is_executed_and_result_stays_valid()
        => (await Context.Valid.ActAsync(Actions.SuccessAsync, Context.Update))
       .Should().BeValid()
       .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_failing_makes_result_invalid()
        => (await Context.Valid.ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("FailureAsync"));
}

public class Task_Valid_Model
{
    [Test]
    public async Task ActAsync_on_sync_success_is_executed_and_result_stays_valid()
        => (await Context.Valid.Async().ActAsync(Actions.Success, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_async_success_is_executed_and_result_stays_valid()
        => (await Context.Valid.Async().ActAsync(Actions.SuccessAsync, Context.Update))
        .Should().BeValid()
        .And.Value.Updated.Should().BeTrue();

    [Test]
    public async Task ActAsync_on_sync_failing_makes_result_invalid()
        => (await Context.Valid.Async().ActAsync(Actions.Failure, Context.Update))
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("Failure"));

    [Test]
    public async Task ActAsync_on_async_failing_makes_result_invalid()
        => (await Context.Valid.Async().ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("FailureAsync"));
}

public class Invalid_Model
{
    [Test]
    public void Act_is_never_triggered()
        => Context.Invalid.Act(Actions.Failure, Context.Update)
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("InvalidContext"));

    [Test]
    public async Task ActAsync_is_never_triggered()
        => (await Context.Invalid.ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("InvalidContext"));
}

public class Task_Invalid_Model
{
    [Test]
    public async Task ActAsync_with_sync_is_never_triggered()
        => (await Context.Invalid.Async().ActAsync(Actions.Failure, Context.Update))
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("InvalidContext"));

    [Test]
    public async Task ActAsync_with_async_is_never_triggered()
        => (await Context.Invalid.Async().ActAsync(Actions.FailureAsync, Context.Update))
        .Should().BeInvalid()
        .WithMessage(ValidationMessage.Error("InvalidContext"));
}

internal class Context
{
    public static Task<Result<Context>> NullTask => Task.FromResult<Result<Context>>(null!);
    public static Result<Context> Valid => Result.For(new Context());
    public static Result<Context> Invalid => Result.WithMessages<Context>(ValidationMessage.Error("InvalidContext"));

    public string? Value { get; set; }
    public bool Updated => Value is { };

    public static void Update(Context context, string value) => context.Value = value;
}

internal static class Actions
{
    public static Result<string> Success(Context _)
        => Result.For(nameof(Success));

    public static AsyncResult_string SuccessAsync(Context _)
        => Result.For(nameof(SuccessAsync)).Async();
    
    public static Result<string> Failure(Context _) 
        => Result.WithMessages<string>(ValidationMessage.Error(nameof(Failure)));

    public static AsyncResult_string FailureAsync(Context _)
        => Result.WithMessages<string>(ValidationMessage.Error(nameof(FailureAsync))).Async();
}
