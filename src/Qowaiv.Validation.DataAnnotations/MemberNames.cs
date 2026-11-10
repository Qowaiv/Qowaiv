namespace Qowaiv.Validation.DataAnnotations;

[DebuggerDisplay("Count = {Count}")]
internal readonly struct MemberNames(string memberName) : IReadOnlyCollection<string>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly MemberPath Path = MemberPath.Root.Child(memberName);

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly List<string> Members = [];

    /// <inheritdoc />
    public int Count => Members.Count;

    public void AddIndex(int index) => Members.Add(Path.Index(index));

    /// <inheritdoc />
    [Pure]
    public IEnumerator<string> GetEnumerator() => Members.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
