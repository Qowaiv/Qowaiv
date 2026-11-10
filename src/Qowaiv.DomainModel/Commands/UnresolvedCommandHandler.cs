namespace Qowaiv.DomainModel.Commands;

/// <summary>The exception that is thrown when a command handler could not be resolved.</summary>
[Serializable]
public class UnresolvedCommandHandler : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="UnresolvedCommandHandler"/> class.</summary>
    [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
    public UnresolvedCommandHandler() : this(QowaivDomainModelMessages.UnresolvedCommandHandler) { }

    /// <summary>Initializes a new instance of the <see cref="UnresolvedCommandHandler"/> class.</summary>
    [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
    public UnresolvedCommandHandler(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="UnresolvedCommandHandler"/> class.</summary>
    [ExcludeFromCodeCoverage/* Required Exception constructor for inheritance. */]
    public UnresolvedCommandHandler(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>When the type could not be resolved.</summary>
    [Pure]
    internal static UnresolvedCommandHandler Type(Type type)
        => new(string.Format(CultureInfo.CurrentCulture, QowaivDomainModelMessages.UnresolvedCommandHandler_Type, type));

    /// <summary>When the method could not be resolved.</summary>
    [Pure]
    internal static UnresolvedCommandHandler Method(Type returnType, Type handlerType, string methodName, Type commandType)
        => new(string.Format(
            CultureInfo.CurrentCulture,
            QowaivDomainModelMessages.UnresolvedCommandHandler_Method,
            returnType,
            handlerType,
            methodName,
            commandType));
}
