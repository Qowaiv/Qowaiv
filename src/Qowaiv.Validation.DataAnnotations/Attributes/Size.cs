using Qowaiv.Extensions;
using Qowaiv.IO;
using Qowaiv.Validation.DataAnnotations.Reflection;
using System.IO;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validation attributes on the size of a value.</summary>
public static partial class Size
{
    [Pure]
    private static StreamSize? GetSize<TAttribute>(object? value) where TAttribute : ValidationAttribute => value switch
    {
        null => null,
        Stream stream => stream.GetStreamSize(),
        ICollection<byte> collection => collection.Count,
        IReadOnlyCollection<byte> collection => collection.Count,
        _ when value.GetType().FullName == "System.BinaryData" && value.TryLength() is { } length => length,
        _ => throw UnsupportedType.ForAttribute<TAttribute>(value.GetType()),
    };
}
