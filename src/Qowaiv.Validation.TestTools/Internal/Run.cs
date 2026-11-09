using System.Threading;
using System.Threading.Tasks;

namespace Qowaiv.Validation.TestTools;

/// <summary>
/// Helpers to run tasks synchronously.
/// </summary>
/// <remarks>
/// Microsoft built an AsyncHelper (internal) class to run Async as Sync.
/// This implementation is equal to that one, besides some naming.
/// </remarks>
internal static class Run
{
    /// <summary>
    /// Run a task synchronously.
    /// </summary>
    /// <param name="task">A callback that returns the task.</param>
    /// <typeparam name="TResult">The type of result of the task.</typeparam>
    /// <returns>The task result.</returns>
    [Pure]
    public static TResult Sync<TResult>(Func<Task<TResult>> task) => Factory
        .StartNew(task, CancellationToken.None)
        .Unwrap()
        .GetAwaiter()
        .GetResult();

    private static readonly TaskFactory Factory = new(
       CancellationToken.None,
       TaskCreationOptions.None,
       TaskContinuationOptions.None,
       TaskScheduler.Default);
}
