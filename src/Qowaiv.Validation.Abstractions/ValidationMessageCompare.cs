namespace Qowaiv.Validation.Abstractions;

/// <summary><see cref="IEqualityComparer{IValidationMessage}"/> helper.</summary>
public static class ValidationMessageCompare
{
    /// <summary>Compares two instances of <see cref="IValidationMessage"/> based on the interface contract.</summary>
    public static readonly IEqualityComparer<IValidationMessage> ByInterface = new CompareByInterface();

    /// <summary>Compares two instances of <see cref="IValidationMessage"/> using <see cref="EqualityComparer{IValidationMessage}.Default"/>.</summary>
    public static readonly IEqualityComparer<IValidationMessage> Default = EqualityComparer<IValidationMessage>.Default;

    private sealed class CompareByInterface : IEqualityComparer<IValidationMessage>
    {
        [Pure]
        public bool Equals(IValidationMessage? x, IValidationMessage? y)
            => x is null || y is null
            ? ReferenceEquals(x, y)
            : Same(x, y);

        [Pure]
        public int GetHashCode(IValidationMessage obj) => obj is null ? 0 : Hash(obj);

        [Pure]
        private static bool Same(IValidationMessage x, IValidationMessage y)
            => x.Message == y.Message
            && x.Severity == y.Severity
            && x.PropertyName == y.PropertyName;

        [Pure]
        private static int Hash(IValidationMessage obj)
            => (obj.Message?.GetHashCode() ?? 0)
            ^ (obj.PropertyName?.GetHashCode() ?? 0)
            ^ (int)obj.Severity;
    }
}
