using System.Text;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Deals with constructing nested member paths.</summary>
/// <remarks>
/// This struct reuses the <see cref="StringBuilder"/> and is not thread-safe.
/// Given its usage this should not be a problem.
/// </remarks>
internal readonly struct MemberPath
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly StringBuilder Builder;

    /// <summary>The (claimed) length of the path.</summary>
    public readonly int Length;

    private MemberPath(StringBuilder sb, int length)
    {
        Builder = sb;
        Length = length;
    }

    /// <summary>Creates a nested path.</summary>
    /// <param name="name">
    /// The name to add to the path.
    /// </param>
    [Pure]
    public MemberPath Child(string name)
    {
        Builder.Length = Length;

        if (Builder.Length != 0)
        {
            Builder.Append('.');
        }

        Builder.Append(name);
        return new(Builder, Builder.Length);
    }

    /// <summary>Creates a nested path.</summary>
    /// <param name="index">
    /// The index of item in the collection.
    /// </param>
    [Pure]
    public MemberPath Child(int index)
    {
        Builder.Length = Length;
        Builder.Append('[').Append(index).Append(']');
        return new(Builder, Builder.Length);
    }

    /// <summary>Creates a path to a property.</summary>
    /// <param name="name">
    /// The name of the property.
    /// </param>
    [Pure]
    public string Property(string name = "")
    {
        Builder.Length = Length;
        if (name.Length == 0)
        {
            return Builder.ToString();
        }
        if (Builder.Length != 0)
        {
            Builder.Append('.');
        }
        Builder.Append(name);
        return Builder.ToString();
    }

    /// <summary>Creates a path to a property.</summary>
    /// <param name="index">
    /// The index to add.
    /// </param>
    [Pure]
    public string Index(int index)
    {
        Builder.Length = Length;
        Builder.Append('[').Append(index).Append(']');
        return Builder.ToString();
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Property();

    /// <summary>Creates a new root instance.</summary>
    [Pure]
    public static MemberPath Root => new(new StringBuilder(), 0);
}
