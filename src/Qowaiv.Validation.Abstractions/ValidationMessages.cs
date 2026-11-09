namespace Qowaiv.Validation.Abstractions;

/// <summary>Represents a read-only collection of valiation messages.</summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(TypeProxy))]
[CollectionBuilder(typeof(ValidationMessages), nameof(New))]
public readonly struct ValidationMessages : IReadOnlyCollection<IValidationMessage>, ICollection
{
    /// <summary>An empty validation message collection.</summary>
    public static readonly ValidationMessages None = new(0, 0, 0, []);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IValidationMessage[] Messages;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int Err;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int Wrn;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly int Inf;

    private ValidationMessages(int err, int wrn, int inf, IValidationMessage[] messages)
    {
        Err = err;
        Wrn = wrn;
        Inf = inf;
        Messages = messages;
    }

    /// <summary>True if any of the messages is an error.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasErrors => Err is not 0;

    /// <summary>Indicates that the collection is not empty.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasAny => Messages is { Length: > 0 };

    /// <inheritdoc />
    public int Count => Messages?.Length ?? 0;

    /// <summary>Gets all messages with <see cref="ValidationSeverity.Error"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ValidationMessages Errors => new(Err, 0, 0, messages(..Err));

    /// <summary>Gets all messages with <see cref="ValidationSeverity.Warning"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ValidationMessages Warnings => new(0, Wrn, 0, messages(Err..^Inf));

    /// <summary>Gets all messages with <see cref="ValidationSeverity.Info"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ValidationMessages Infos => new(0, 0, Inf, messages(^Inf..));

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool ICollection.IsSynchronized => true;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    object ICollection.SyncRoot => messages().SyncRoot;

    /// <summary>Creates a new validation message collection.</summary>
    /// <param name="messages">
    /// The messages to append.
    /// </param>
    [Pure]
    public ValidationMessages Append(IEnumerable<IValidationMessage> messages)
        => messages is null
        ? this
        : New(messages, [.. this.messages()], Err, Wrn, Inf);

    /// <inheritdoc />
    public void CopyTo(Array array, int index)
    {
        if (HasAny) Messages.CopyTo(array, index);
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerator<IValidationMessage> GetEnumerator()
        => Messages is null
        ? Enumerable.Empty<IValidationMessage>().GetEnumerator()
        : ((IEnumerable<IValidationMessage>)Messages).GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Helper to get a subset of all messages.</summary>
    /// <remarks>
    /// Required as .NET standard 2.0 does not support SubRange(Range) on collections.
    /// </remarks>
    [Pure]
#if NETSTANDARD2_0
    private IValidationMessage[] messages(Range range)
    {
        // SubRange is not available for .NET standard 2.0 unfortunatly.
        var count = Count;
        var start = range.Start is { IsFromEnd: true } ? count - range.Start.Value : range.Start.Value;
        var end = range.End is { IsFromEnd: true } ? count - range.End.Value : range.End.Value;
        var length = end - start;

        if (length == count) return messages();

        var copy = new IValidationMessage[length];
        Array.Copy(Messages, start, copy, 0, length);
        return copy;
    }
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IValidationMessage[] messages(Range range) => messages()[range];
#endif

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IValidationMessage[] messages() => Messages ?? [];

    /// <summary>Creates a new collection of <see cref="ValidationMessages" />.</summary>
    /// <param name="messages">
    /// The messages.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When the message Severity is other then Error, Warning, Info, or None.
    /// </exception>
    [Pure]
    [OverloadResolutionPriority(1)]
    public static ValidationMessages New(IEnumerable<IValidationMessage?> messages)
        => messages is null
        ? None
        : New(messages, [], 0, 0, 0);

    /// <summary>Helper to reduce allocations for addition.</summary>
    [Pure]
    private static ValidationMessages New(IEnumerable<IValidationMessage?> messages, List<IValidationMessage> list, int err, int wrn, int inf)
    {
        var index = -1;
        foreach (var message in messages)
        {
            index++;
            if (message is null || message.Severity is ValidationSeverity.None) continue;

            if (message.Severity is < ValidationSeverity.None or > ValidationSeverity.Error)
                throw new ArgumentOutOfRangeException($"messages[{index}]", $"Severity {message.Severity} is not Error, Warning, Info, or None.");

            list.Add(message);

            switch (message.Severity)
            {
                case ValidationSeverity.Error: err++; break;
                case ValidationSeverity.Warning: wrn++; break;
                case ValidationSeverity.Info: inf++; break;
            }
        }

        list.Sort(Compare);

        return new(err, wrn, inf, [.. list]);
    }

    /// <inheritdoc  cref="New(IEnumerable{IValidationMessage?})"/>
    [Pure]
    public static ValidationMessages New(ReadOnlySpan<IValidationMessage> messages) => New(messages.ToArray());

    /// <summary>Sorts based on Severity descending.</summary>
    [Pure]
    private static int Compare(IValidationMessage l, IValidationMessage r) => r.Severity.CompareTo(l.Severity);

    /// <remarks>
    /// This helps to show the Errors, Warnings, and Infos in the debugger display.
    /// </remarks>
    private sealed class TypeProxy(IReadOnlyCollection<IValidationMessage> messages)
    {
        public List<IValidationMessage> Errors { get; } = [.. messages.Where(m => m.Severity is ValidationSeverity.Error)];

        public List<IValidationMessage> Warnings { get; } = [.. messages.Where(m => m.Severity is ValidationSeverity.Warning)];

        public List<IValidationMessage> Infos { get; } = [.. messages.Where(m => m.Severity is ValidationSeverity.Info)];
    }
}
