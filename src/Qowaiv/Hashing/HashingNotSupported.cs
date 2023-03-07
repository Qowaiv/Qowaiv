namespace Qowaiv.Hashing;

/// <summary>An exception to thrown to indicate that a sertain class/struct
/// is not designed to support hashing.
/// </summary>
[ExcludeFromCodeCoverage]
[Serializable]
public class HashingNotSupported : NotSupportedException
{
    /// <summary>Initializes a new instance of the <see cref="HashingNotSupported"/> class.</summary>
    public HashingNotSupported() { }

    /// <summary>Initializes a new instance of the <see cref="HashingNotSupported"/> class.</summary>
    public HashingNotSupported(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="HashingNotSupported"/> class.</summary>
    public HashingNotSupported(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>Initializes a new instance of the <see cref="HashingNotSupported"/> class.</summary>
    protected HashingNotSupported(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
