using System.Net;
using System.Reflection;

namespace Qowaiv.Validation.Messages;

/// <summary>Message to communicate if a entity could not be found.</summary>
public class EntityNotFound : InvalidOperationException, IHttpStatusMessage
{
    /// <summary>Initializes a new instance of the <see cref="EntityNotFound"/> class.</summary>
    public EntityNotFound() : this(ValidationMessages.EntityNotFound) { }

    /// <summary>Initializes a new instance of the <see cref="EntityNotFound"/> class.</summary>
    public EntityNotFound(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="EntityNotFound"/> class.</summary>
    public EntityNotFound(string message, Exception innerException) : base(message, innerException) { }

    /// <inheritdoc />
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    /// <inheritdoc />
    public ValidationSeverity Severity => ValidationSeverity.Error;

    /// <inheritdoc />
    public string? PropertyName => null;

    /// <summary>The Type of the entity.</summary>
    public Type? EntityType { get; init; }

    /// <summary>Creates an <see cref="EntityNotFound"/> for specific ID.</summary>
    [Pure]
    public static EntityNotFound ForId(object id)
        => new(string.Format(ValidationMessages.EntityNotFound_ForId, id));

    /// <summary>Creates an <see cref="EntityNotFound"/> for specific Entity.</summary>
    [Pure]
    public static EntityNotFound For<TEntity>(object id) where TEntity : notnull
        => For(id, typeof(TEntity));

    /// <summary>Creates an <see cref="EntityNotFound"/> for specific Entity.</summary>
    [Pure]
    public static EntityNotFound For(object id, Type entityType)
        => new(string.Format(ValidationMessages.EntityNotFound_ForTypeAndId, TypeName(entityType), id))
        {
            EntityType = entityType,
        };

    /// <summary>Tries to resolves Qowaiv's Type.ToCSharpString(bool) and falls back to <see cref="Type.FullName"/>.</summary>
    private static Func<Type, string> TypeName => field ??= Type
        .GetType("System.QowaivTypeExtensions, Qowaiv")?
        .GetMethods(BindingFlags.Static | BindingFlags.Public)
        .FirstOrDefault(m => m.Name == "ToCSharpString" && m.GetParameters().Length == 2) is { } m
            ? t => m.Invoke(null, [t, true])?.ToString() ?? string.Empty
            : t => t.FullName ?? t.Name;
}
