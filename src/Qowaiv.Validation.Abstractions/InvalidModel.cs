namespace Qowaiv.Validation.Abstractions;

/// <summary>Represents an exception that is shown once tried to access the invalid model of a <see cref="Result{T}"/>.</summary>
public class InvalidModel : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="InvalidModel"/> class.</summary>
    public InvalidModel() { }

    /// <summary>Initializes a new instance of the <see cref="InvalidModel"/> class.</summary>
    public InvalidModel(string? message)
        : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="InvalidModel"/> class.</summary>
    public InvalidModel(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>Initializes a new instance of the <see cref="InvalidModel"/> class.</summary>
    public InvalidModel(string? message, Exception? innerException, ValidationMessages messages)
        : this(message, innerException)
        => Errors = messages.Errors;

    /// <summary>The related validation error(s).</summary>
    public ValidationMessages Errors { get; } = [];

    /// <summary>Creates an <see cref="InvalidModel"/> for the model.</summary>
    [Pure]
    public static InvalidModel For<T>(IEnumerable<IValidationMessage> messages)
    {
        var filtered = ValidationMessages.New(messages).Errors;

        var sb = new StringBuilder().AppendFormat(QowaivValidationMessages.InvalidModel, typeof(T));

        if (filtered.HasAny)
        {
            sb.Remove(sb.Length - 1, 1).AppendLine(":");
            foreach (var message in filtered)
            {
                Append(sb, message);
            }
        }
        return new InvalidModel(sb.ToString(), null, filtered);
    }

    /// <summary>Creates an <see cref="InvalidModel"/> for the model.</summary>
    [Pure]
    public static InvalidModel For(IEnumerable<IValidationMessage> messages)
    {
        var filtered = ValidationMessages.New(messages).Errors;

        var sb = new StringBuilder().Append(QowaivValidationMessages.InvalidOperationException);

        if (filtered.HasAny)
        {
            sb.Remove(sb.Length - 1, 1).AppendLine(":");
            foreach (var message in filtered)
            {
                Append(sb, message);
            }
        }
        return new InvalidModel(sb.ToString(), null, filtered);
    }

    private static void Append(StringBuilder builder, IValidationMessage message)
    {
        var text = message.Message is { Length: > 0 }
            ? message.Message.Trim()
            : "Validation Error.";
        var lines = text.Split(NewLine, StringSplitOptions.None);
        builder
            .Append("* ")
            .Append(string.Join(Environment.NewLine + "  ", lines))
            .AppendLine(PropertySuffix(message));
    }

    [Pure]
    private static string PropertySuffix(IValidationMessage message)
        => string.IsNullOrWhiteSpace(message.PropertyName) ? string.Empty : $" ({message.PropertyName})";

    private static readonly string[] NewLine = ["\r\n", "\n"];
}
