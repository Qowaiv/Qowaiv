namespace Qowaiv.Diagnostics
{
    /// <summary>Helper class to decorate references that will never be null.</summary>
    [ExcludeFromCodeCoverage]
    internal static class Not
    {
        /// <summary>Promises the reference will never be null.</summary>
        /// <remarks>
        /// Should only be used to satisfy the compiler, when we are 100% sure, there can not be a null dereference.
        /// </remarks>
        [Pure]
        public static T Null<T>(T? reference) => reference ?? throw new NullReferenceException();
    }
}
