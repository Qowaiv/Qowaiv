namespace Qowaiv.Validation.Abstractions;

/// <summary>Manimpulation of collections of <see cref="Result"/>s.</summary>
public static class Results
{
    extension(IEnumerable<Result> results)
    {
        /// <summary>Combines the results.</summary>
        [Pure]
        public Result Combine()
            => new([.. results.SelectMany(r => r.Messages)]);
    }

    extension(IEnumerable<AsyncResult> results)
    {
        /// <summary>Combines the results.</summary>
        [Pure]
        public async AsyncResult Combine()
        {
            var promises = await Task.WhenAll(Guard.NotNull(results));
            return promises.Combine();
        }
    }
}
