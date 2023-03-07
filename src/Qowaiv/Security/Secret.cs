using Qowaiv.Conversion.Security;
using Qowaiv.Security.Cryptography;
using System.Security.Cryptography;

namespace Qowaiv.Security;

/// <summary>Represents a secret.</summary>
[TypeConverter(typeof(SecretTypeConverter))]
[DebuggerDisplay("{DebuggerDisplay}")]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Security.SecretJsonConverter))]
#endif
public readonly struct Secret : IEquatable<Secret>
{
    /// <summary>Represents an empty/not set secret.</summary>
    public static readonly Secret Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string m_Value;

    private Secret(string value) => m_Value = value;

    /// <summary>Returns true if the secret is empty/not set.</summary>
    [Pure]
    public bool IsEmpty() => m_Value is null;

    /// <summary>Gets a string representing this secret.</summary>
    [Pure]
    public string Value() => m_Value ?? string.Empty;

    /// <summary>Computes a <see cref="CryptographicSeed"/> for the secret.</summary>
    /// <param name="algorithm">
    /// The algorithm to hash with.
    /// </param>
    [Pure]
    public CryptographicSeed ComputeHash(HashAlgorithm algorithm)
        => algorithm.ComputeCryptographicSeed(Encoding.UTF8.GetBytes(Value()));

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Secret other && Equals(other);

    /// <summary>Returns true if both are empty, otherwise false.</summary>
    /// <remarks>
    /// Secrets are supposed to be passed around, without knowing its content.
    /// </remarks>
    [Pure]
    public bool Equals(Secret other) => IsEmpty() && other.IsEmpty();

    /// <inheritdoc />
    [Pure]
#if NET5_0_OR_GREATER
    [DoesNotReturn]
#endif
    public override int GetHashCode() => Hash.NotSupportedBy<Secret>();

    /// <summary>Represents the secret as "*****".</summary>
    /// <remarks>
    /// To prevent unintended exposure.
    /// </remarks>
    [Pure]
    public override string ToString() => m_Value is null ? string.Empty : "*****";

    /// <summary>Converts the secret to a JSON null node.</summary>
    /// <remarks>
    /// To prevent unintended exposure.
    /// </remarks>
    [Pure]
#pragma warning disable CA1822 // part of the contract
    public object? ToJson() => null;
#pragma warning restore CA1822

    /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => m_Value is null ? DebugDisplay.Empty : Value();

    /// <summary>Creates a secret from an UTF8 string.</summary >
    /// <param name="str" >
    /// A secret describing a cryptographic seed.
    /// </param >
    [Pure]
    public static Secret Parse(string? str)
        => str is not { Length: > 0 }
        ? Empty
        : new(str);

    /// <summary>Creates a secret from a JSON string node.</summary>
    [Pure]
    public static Secret FromJson(string? json) => Parse(json);
}
