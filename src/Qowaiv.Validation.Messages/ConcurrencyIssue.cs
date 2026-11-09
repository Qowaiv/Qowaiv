using System.Net;

namespace Qowaiv.Validation.Messages;

/// <summary>Message to communicate concurrency control failed.</summary>
public class ConcurrencyIssue : InvalidOperationException, IHttpStatusMessage
{
    /// <summary>Initializes a new instance of the <see cref="ConcurrencyIssue"/> class.</summary>
    public ConcurrencyIssue() : base(ValidationMessages.ConcurrencyIssue) { }

    /// <summary>Initializes a new instance of the <see cref="ConcurrencyIssue"/> class.</summary>
    public ConcurrencyIssue(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="ConcurrencyIssue"/> class.</summary>
    public ConcurrencyIssue(string message, Exception innerException) : base(message, innerException) { }

    /// <inheritdoc />
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    /// <inheritdoc />
    public ValidationSeverity Severity => ValidationSeverity.Error;

    /// <inheritdoc />
    public string? PropertyName => null;

    /// <summary>Creates an <see cref="ConcurrencyIssue"/> for the version mismatch.</summary>
    [Pure]
    public static ConcurrencyIssue VersionMismatch(object expectedVersion, object actualVersion)
        => new(string.Format(
            CultureInfo.CurrentCulture,
            ValidationMessages.VersionMismatch,
            expectedVersion,
            actualVersion));

    /// <summary>Creates an <see cref="ConcurrencyIssue"/> for mid-air collision.</summary>
    [Pure]
    public static ConcurrencyIssue MidAirCollision()
        => new(ValidationMessages.MidAirCollision);

    /// <summary>Creates an <see cref="ConcurrencyIssue"/> for mid-air collision.</summary>
    [Pure]
    public static ConcurrencyIssue MidAirCollision(Exception innerException)
        => new(ValidationMessages.MidAirCollision, innerException);
}
