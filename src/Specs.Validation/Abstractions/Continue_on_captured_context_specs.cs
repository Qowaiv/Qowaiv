using System.Threading;
using Qowaiv.Validation.Abstractions;


namespace Abstractions.Continue_on_captured_context_specs;

public class Continue_on_captured_context_false
{
    [Test]
    public void AsyncResult_chain_does_not_post_to_the_captured_context()
        => ContextProbe.Posts(() => Result.For(1).Async().ActAsync(Delayed.Action, continueOnCapturedContext: false).GetAwaiter().GetResult())
        .Should().Be(0);

    [Test]
    public void AsyncResult_chain_of_non_generic_actions_does_not_post_to_the_captured_context()
        => ContextProbe.Posts(() => Result.For(1).Async().ActAsync(Delayed.ActionResult, continueOnCapturedContext: false).GetAwaiter().GetResult())
        .Should().Be(0);

    [Test]
    public void AsyncResult_chain_with_update_does_not_post_to_the_captured_context()
        => ContextProbe.Posts(() => Result.For(1).Async().ActAsync(Delayed.Action, static (_, _) => { }, continueOnCapturedContext: false).GetAwaiter().GetResult())
        .Should().Be(0);

    [Test]
    public void AsyncResult_chain_with_func_update_does_not_post_to_the_captured_context()
        => ContextProbe.Posts(() => Result.For(1).Async().ActAsync(Delayed.Action, static (current, _) => current + 1, continueOnCapturedContext: false).GetAwaiter().GetResult())
        .Should().Be(0);

    [Test]
    public void Result_chain_with_update_does_not_post_to_the_captured_context()
        => ContextProbe.Posts(() => Result.For(1).ActAsync(Delayed.Action, static (current, _) => current + 1, continueOnCapturedContext: false).GetAwaiter().GetResult())
        .Should().Be(0);
}

public class Continue_on_captured_context_true
{
    [Test]
    public void AsyncResult_chain_posts_to_the_captured_context()
        => ContextProbe.Posts(() => Result.For(1).Async().ActAsync(Delayed.Action, continueOnCapturedContext: true).GetAwaiter().GetResult())
        .Should().BePositive();
}

internal static class Delayed
{
    public static AsyncResult_string Action(int _)
        => ActionAsync();

    public static AsyncResult ActionResult(int _)
        => ActionResultAsync();

    private static async Task<Result<string>> ActionAsync()
    {
        await Task.Delay(10).ConfigureAwait(false);
        return Result.For(nameof(ActionAsync));
    }

    private static async AsyncResult ActionResultAsync()
    {
        await Task.Delay(10).ConfigureAwait(false);
        return Result.OK;
    }
}

internal static class ContextProbe
{
    public static int Posts(Action chain)
    {
        var previous = SynchronizationContext.Current;
        var context = new CountingSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(context);
        try
        {
            chain();
            return context.Posts;
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(previous);
        }
    }
}

internal sealed class CountingSynchronizationContext : SynchronizationContext
{
    private int _posts;

    public int Posts => Volatile.Read(ref _posts);

    public override void Post(SendOrPostCallback d, object? state)
    {
        Interlocked.Increment(ref _posts);
        ThreadPool.UnsafeQueueUserWorkItem(_ =>
        {
            var previous = Current;
            SetSynchronizationContext(this);
            try
            {
                d(state);
            }
            finally
            {
                SetSynchronizationContext(previous);
            }
        }, null);
    }
}
