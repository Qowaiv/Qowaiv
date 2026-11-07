using Qowaiv.Reflection;
using System.Reflection;

namespace Qowaiv.TestTools.Conversion;

/// <summary>Describes a type conversion.</summary>
public sealed class TypeConversion : IEquatable<TypeConversion>
{
    /// <summary>Initializes a new instance of the <see cref="TypeConversion"/> class.</summary>
    public TypeConversion(Type sourceType, Type targetType)
    {
        SourceType = Guard.NotNull(sourceType);
        TargetType = Guard.NotNull(targetType);

        lazyCast = new(() => new[] { sourceType, targetType }
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .FirstOrDefault(m => m is { IsSpecialName: true, Name: "op_Implicit" or "op_Explicit" }
                && m.ReturnType == targetType
                && m.GetParameters() is { Length: 1 } pars
                && pars[0].ParameterType == sourceType));

        lazyConverter = new(()
            => TypeDescriptor.GetConverter(targetType) is { } converter
            && converter.CanConvertFrom(sourceType)
            ? converter
            : null);
    }

    /// <summary>The type of the source.</summary>
    public Type SourceType { get; }

    /// <summary>The type of the target.</summary>
    public Type TargetType { get; }

    /// <summary>The (implict or explict) cast operator (if existing).</summary>
    public MethodInfo? Cast => lazyCast.Value;

    /// <summary>The <see cref="System.ComponentModel.TypeConverter"/> that can do the conversion.</summary>
    public TypeConverter? TypeConverter => lazyConverter.Value;

    /// <summary>True if the source type equals the target type.</summary>
    public bool IsIdentity => SourceType == TargetType;

    /// <summary>True if the target type is assignable from the source type.</summary>
    public bool IsUpcast => TargetType.IsAssignableFrom(SourceType);

    /// <summary>To deconstruct only the source and target type.</summary>
    public void Deconstruct(out Type sourceType, out Type targetType)
    {
        sourceType = SourceType;
        targetType = TargetType;
    }

    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TypeConversion other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(TypeConversion? other)
        => other is { }
        && SourceType == other.SourceType
        && TargetType == other.TargetType;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => SourceType.GetHashCode() ^ (TargetType.GetHashCode() >> 5);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{SourceType.ToCSharpString()} => {TargetType.ToCSharpString()}";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Lazy<MethodInfo?> lazyCast;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Lazy<TypeConverter?> lazyConverter;
}
