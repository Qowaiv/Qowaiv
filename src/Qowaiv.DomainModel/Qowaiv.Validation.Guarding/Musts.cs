namespace Qowaiv.Validation.Guarding;

/// <summary>Extensions on <see cref="Must{TSubject}"/>.</summary>
public static class DomainModelExtensions
{
    /// <summary>Guards the <see cref="Aggregate{TAggregate, TId}"/> to have the expected version;
    /// otherwise return a <see cref="ConcurrencyIssue"/>.
    /// </summary>
    /// <typeparam name="TAggregate">
    /// The type of the aggregate.
    /// </typeparam>
    /// <param name="must">
    /// The must to extend on.
    /// </param>
    /// <param name="expected">
    /// The expected version.
    /// </param>
    /// <remarks>
    /// A dynamic is used, as the TId generic can not be resolved by usage.
    /// It would be inconvenient to have specify to the generics on this
    /// extension.
    /// </remarks>
    [Pure]
    public static Result<TAggregate> HaveVersion<TAggregate>(this Must<TAggregate> must, long expected)
        where TAggregate : Aggregate<TAggregate>, new()
    {
        Guard.NotNull(must);
        Guard.NotNegative(expected);
        long actual = ((dynamic)must.Subject).Version;
        return must.Be(actual == expected, ConcurrencyIssue.VersionMismatch(expected, actual));
    }
}
