using Qowaiv.Identifiers;
using System.Reflection;

namespace Qowaiv.Conversion.Identifiers;

/// <summary>Provides a conversion for strongly typed identifiers.</summary>
public sealed class IdTypeConverter : TypeConverter
{
    /// <summary>Accessor to the underlying value.</summary>
    private readonly FieldInfo m_Value;

    /// <summary>Accessor to the private constructor.</summary>
    private readonly ConstructorInfo Ctor;

    /// <summary>The base type according to the <see cref="IIdentifierBehavior"/>.</summary>
    private readonly Type BaseType;

    /// <summary>The <see cref="TypeConverter"/> of the underlying value.</summary>
    private readonly TypeConverter BaseConverter;

    /// <summary>Initializes a new instance of the <see cref="IdTypeConverter"/> class.</summary>
    /// <param name="type">
    /// The type to convert for.
    /// </param>
    public IdTypeConverter(Type type)
    {
        Guard.NotNull(type, nameof(type));

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Id<>) && type.GetGenericArguments().Length == 1)
        {
            m_Value = type.GetField(nameof(m_Value), NonPublicInstance)!;
            var ctors = type.GetConstructors(NonPublicInstance);
            Ctor = ctors.First(ctor => ctor.GetParameters().Length == 1);
            var behavior = (IIdentifierBehavior)Activator.CreateInstance(type.GetGenericArguments()[0])!;
            BaseType = behavior.BaseType;
            BaseConverter = behavior.Converter;
        }
        else throw new ArgumentException("Incompatible type", nameof(type));
    }

    private const BindingFlags NonPublicInstance = (BindingFlags)36;

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == BaseType || BaseConverter.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == BaseType || BaseConverter.CanConvertTo(context, destinationType);

    /// <inheritdoc />
    [Pure]
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (string.Empty.Equals(value) || value is null)
        {
            return Ctor.Invoke(new object?[] { null });
        }
        else
        {
            var id = BaseConverter.ConvertFrom(context, culture, value);
            return Ctor.Invoke(new[] { id });
        }
    }

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        var id = m_Value.GetValue(value);
        return destinationType == BaseType
            ? id
            : BaseConverter.ConvertTo(context, culture, id, destinationType);
    }
}
