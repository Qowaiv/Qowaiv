using System.Threading.Tasks;

namespace Qowaiv.Validation.TestTools;

/// <summary>Contains extension methods for custom assertions in unit tests.</summary>
[DebuggerNonUserCode]
public static class Shoulds
{
    extension(Result? result)
    {
        /// <summary>
        /// Returns an <see cref="ResultAssertions"/> object that can be used to assert the
        /// current <see cref="Result"/>.
        /// </summary>
        [Pure]
        public ResultAssertions Should([CallerArgumentExpression(nameof(result))] string? expression = null)
            => new(result, expression);
    }

    extension<TModel>(Result<TModel> result)
    {
        /// <summary>
        /// Returns an <see cref="ResultAssertions{TModel}"/> object that can be used to assert the
        /// current <see cref="Result{TModel}"/>.
        /// </summary>
        [Pure]
        public ResultAssertions<TModel> Should([CallerArgumentExpression(nameof(result))] string? expression = null)
            => new(result, expression);
    }

    extension(Task<Result> promise)
    {
        /// <summary>
        /// Returns an <see cref="ResultAssertions"/> object that can be used to assert the
        /// current task of a <see cref="Result"/>.
        /// </summary>
        [Pure]
        public ResultAssertions Should([CallerArgumentExpression(nameof(promise))] string? expression = null)
            => new(Run.Sync(() => promise), expression);
    }

    extension<TModel>(Task<Result<TModel>> promise)
    {
        /// <summary>
        /// Returns an <see cref="ResultAssertions{TModel}"/> object that can be used to assert the
        /// current task of a <see cref="Result{TModel}"/>.
        /// </summary>
        [Pure]
        public ResultAssertions<TModel> Should([CallerArgumentExpression(nameof(promise))] string? expression = null)
            => new(Run.Sync(() => promise), expression);
    }
}
