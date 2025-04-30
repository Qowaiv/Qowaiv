using System.IO;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents a .NET namespace.</summary>
public readonly struct Namespace(string name) : IEquatable<Namespace>
{
    /// <summary>Empty/non-existent namespace.</summary>
    public static readonly Namespace Empty;

    /// <summary>Gets the name of the namespace.</summary>
    public string Name { get; } = Guard.NotNullOrEmpty(name);

    /// <summary>Get the parent namespace of the namespace.</summary>
    public Namespace Parent
    {
        get
        {
            var index = Name.LastIndexOf('.');
            return index == -1 ? default : new(Name[..index]);
        }
    }

    /// <summary>Returns true if empty.</summary>
    [Pure]
    public bool IsEmpty() => string.IsNullOrEmpty(Name);

    /// <summary>Returns a child namespace based on this one.</summary>
    [Pure]
    public Namespace Child(string child)
        => IsEmpty()
            ? new(child)
            : new($"{Name}.{child}");

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => Name ?? string.Empty;

    /// <inheritdoc/>
    [Pure]
    public override bool Equals(object? obj) => obj is Namespace other && Equals(other);

    /// <inheritdoc/>
    [Pure]
    public bool Equals(Namespace other) => Name == other.Name;

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => Name is null ? 0 : Name.GetHashCode();

    /// <summary>Returns true if both namespaces are the same.</summary>
    public static bool operator ==(Namespace left, Namespace right) => left.Equals(right);

    /// <summary>Returns false if both namespaces are the same.</summary>
    public static bool operator !=(Namespace left, Namespace right) => !(left == right);

    /// <summary>Implicitly casts a string to a namespace.</summary>
    public static implicit operator Namespace(string value) => new(value);

    /// <summary>Creates a collection of global namespaces based on file.</summary>
    [Pure]
    public static IEnumerable<Namespace> Globals(FileInfo file)
        => Globals(Guard.Exists(file).OpenRead());

    /// <summary>Creates a collection of global namespaces based on stream.</summary>
    [Pure]
    public static IEnumerable<Namespace> Globals(Stream stream)
    {
        Guard.NotNull(stream);
        using var reader = new StreamReader(stream);
        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("global using ") && line[^1] == ';')
            {
                yield return new Namespace(line[13..^1]);
            }
        }
    }
}
