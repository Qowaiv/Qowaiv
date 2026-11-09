using System.Net;

namespace Qowaiv.Validation.Messages;

/// <summary>Message to communicate that a payment is required.</summary>
public class PaymentRequired : InvalidOperationException, IHttpStatusMessage
{
    /// <summary>Initializes a new instance of the <see cref="PaymentRequired"/> class.</summary>
    public PaymentRequired() : this(ValidationMessages.PaymentRequired) { }

    /// <summary>Initializes a new instance of the <see cref="PaymentRequired"/> class.</summary>
    public PaymentRequired(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="PaymentRequired"/> class.</summary>
    public PaymentRequired(string message, Exception innerException) : base(message, innerException) { }

    /// <inheritdoc />
    public HttpStatusCode StatusCode => HttpStatusCode.PaymentRequired;

    /// <inheritdoc />
    public ValidationSeverity Severity => ValidationSeverity.Error;

    /// <inheritdoc />
    public string? PropertyName { get; set; }
}
