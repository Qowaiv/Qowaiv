using System.Net;

namespace Qowaiv.Validation.Abstractions;

/// <summary>Represents a validation message that can be bound to an HTTP status.</summary>
public interface IHttpStatusMessage : IValidationMessage
{
    /// <summary>Gets the HTTP status code that best describes the message.</summary>
    HttpStatusCode StatusCode { get; }
}
