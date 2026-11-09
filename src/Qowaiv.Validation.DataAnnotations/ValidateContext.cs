using Qowaiv.Validation.Abstractions;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Represents a context to validate models.</summary>
internal sealed class ValidateContext(IServiceProvider? serviceProvider, IDictionary<object, object?>? items)
{
    /// <summary>The collected messages.</summary>
    public readonly List<IValidationMessage> Messages = [];

    /// <summary>Keeps track of objects that already have been validated.</summary>
    public readonly HashSet<object> Done = [with(ReferenceComparer.Instance)];

    /// <summary>The service provider for the <see cref="ValidationContext"/>.</summary>
    private readonly IServiceProvider? ServiceProvider = serviceProvider;

    /// <summary>The service items for the <see cref="ValidationContext"/>.</summary>
    private readonly IDictionary<object, object?> Items = items ?? new Dictionary<object, object?>(capacity: 0);

    /// <summary>Adds a set of messages.</summary>
    public void AddMessages(IEnumerable<ValidationResult> messages, MemberPath path)
    {
        foreach (var message in messages)
        {
            AddMessage(message, path);
        }
    }

    /// <summary>Adds a message.</summary>
    /// <returns>
    /// True if the message had a severity.
    /// </returns>
    /// <remarks>
    /// Null and <see cref="ValidationMessage.None"/> Messages are not added.
    /// </remarks>
    [Impure]
    public bool AddMessage(ValidationResult? validationResult, MemberPath path)
    {
        if (validationResult is null) return false;

        var added = false;

        // We want to have a message per member name
        foreach (var member in validationResult.MemberNames.Where(m => m is { Length: > 0 }))
        {
            added = true;

            Messages.Add(validationResult is ValidationMessage m
                ? ValidationMessage.For(m.Severity, m.Message!, path.Property(member))!
                : ValidationMessage.Error(validationResult.ErrorMessage, path.Property(member)));
        }

        // If no member name has been specified, we add one message with an updated property.
        if (!added)
        {
            Messages.Add(validationResult is ValidationMessage m
                ? ValidationMessage.For(m.Severity, m.Message!, path.Property())!
                : ValidationMessage.Error(validationResult.ErrorMessage, path.Property()));
        }

        return true;
    }

    /// <summary>Gets a <see cref="ValidationContext"/> to validate the nested instance with.</summary>
    /// <remarks>
    /// A context is only created when <see cref="TypeAnnotations.NeedsContext"/> is true.
    /// Otherwise a single shared (per validation) context is returned, as the
    /// <see cref="ValidationContext.MemberName"/> and <see cref="ValidationContext.DisplayName"/>
    /// are the only mutable members used by attributes that do not require the context.
    /// </remarks>
    [Impure]
    public ValidationContext Validation(Nested nested)
        => nested.Annotations.NeedsContext
        ? new(nested.Instance, ServiceProvider, Items)
        : None ??= new(nested.Instance, ServiceProvider, Items);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ValidationContext? None;
}
