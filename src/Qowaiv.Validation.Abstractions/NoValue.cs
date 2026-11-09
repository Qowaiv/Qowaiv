namespace Qowaiv.Validation.Abstractions;

/// <summary>Exception to communicate that <see cref="Result{TModel}.Value"/> has no value (unintentionally).</summary>
public class NoValue : ArgumentNullException
{
    /// <summary>Initializes a new instance of the <see cref="NoValue"/> class.</summary>
    public NoValue() : this(nameof(Result<>.Value), QowaivValidationMessages.NoValue) { }

    /// <summary>Initializes a new instance of the <see cref="NoValue"/> class.</summary>
    public NoValue(string paramName) : base(paramName) { }

    /// <summary>Initializes a new instance of the <see cref="NoValue"/> class.</summary>
    public NoValue(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>Initializes a new instance of the <see cref="NoValue"/> class.</summary>
    public NoValue(string paramName, string message) : base(paramName, message) { }

    /// <summary>Creates a new <see cref="NoValue"/> exception for a <see cref="Result{TModel}"/>.</summary>
    /// <typeparam name="T">
    /// The type of the result.
    /// </typeparam>
    [Pure]
    public static NoValue For<T>()
        => new(nameof(Result<>.Value), string.Format(QowaivValidationMessages.NoValue_ForT, typeof(T).Name));
}
