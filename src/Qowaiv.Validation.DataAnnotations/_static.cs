namespace Qowaiv.Validation.DataAnnotations;

internal static class Statics
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] array<T>(params IEnumerable<T> values) => [.. values];
}
