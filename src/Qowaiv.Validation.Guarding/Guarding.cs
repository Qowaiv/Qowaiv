namespace Qowaiv.Validation.Guarding;

/// <summary>Extension method to define guarding conditions.</summary>
public static class Guarding
{
    /// <typeparam name="TSubject">
    /// The type of the subject.
    /// </typeparam>
    extension<TSubject>(TSubject subject) where TSubject : class
    {
        /// <summary>Creates a <see cref="Must{TSubject}"/> for this object.</summary>
        [Pure]
        public Must<TSubject> Must() => new(subject);
    }
}
