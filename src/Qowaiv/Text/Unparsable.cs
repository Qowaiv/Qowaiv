namespace Qowaiv.Text;

/// <summary>The exception that is thrown when a value could not be parsed.</summary>
/// <remarks>
/// This child type allows to specify the string value and the target type involved.
/// </remarks>
public class Unparsable : FormatException
{
    /// <summary>Initializes a new instance of the <see cref="Unparsable"/> class.</summary>
    public Unparsable() { }

    /// <summary>Initializes a new instance of the <see cref="Unparsable"/> class.</summary>
    public Unparsable(string? message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="Unparsable"/> class.</summary>
    public Unparsable(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>Initializes a new instance of the <see cref="Unparsable"/> class.</summary>
    protected Unparsable(SerializationInfo info, StreamingContext context) : base(info, context) 
    {
        Type = info.GetString(nameof(Type));
        Value = info.GetString(nameof(Value));
    }

    /// <inheritdoc />
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Type), Type);
        info.AddValue(nameof(Value), Value);
    }

    /// <summary>The target type.</summary>
    public string? Type { get; init; }

    /// <summary>The value that could not be parsed.</summary>
    public string? Value { get; init; }

    /// <summary>Creates an <see cref="Unparsable"/> </summary>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    /// <param name="value">
    /// The string value to parse.
    /// </param>
    /// <param name="message">
    /// The exception message.
    /// </param>
    [Pure]
    public static Unparsable ForValue<TTarget>(string? value, string message)
        => new(message)
        {
            Type = typeof(TTarget).ToCSharpString(),
            Value = value,
        };
}
