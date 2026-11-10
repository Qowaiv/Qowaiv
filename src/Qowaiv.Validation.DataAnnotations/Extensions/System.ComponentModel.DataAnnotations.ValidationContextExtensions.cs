namespace System.ComponentModel.DataAnnotations;

/// <summary>Extensions on <see cref="ValidationContext"/>.</summary>
public static class ValidationContextExtensions
{
    extension(ValidationContext validationContext)
    {
        /// <summary>Returns the service that provides custom validation.</summary>
        [Pure]
        public T? GetService<T>()
            => (T?)Guard.NotNull(validationContext).GetService(typeof(T));

        /// <summary>Gets the <see cref="ValidationContext.MemberName"/> as an array.</summary>
        [Pure]
        internal string[] MemberNames()
            => validationContext.MemberName is { Length: > 0 }
            ? [validationContext.MemberName]
            : [];
    }
}
