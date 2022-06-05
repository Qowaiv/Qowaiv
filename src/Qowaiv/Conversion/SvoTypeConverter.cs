using System.Reflection;

namespace Qowaiv.Conversion;

/// <summary>Provides a conversion for strongly typed identifiers.</summary>
public sealed class SvoTypeConverter : TypeConverter
{
    /// <summary>Accessor to the underlying value.</summary>
    private readonly FieldInfo m_Value;

    /// <summary>Accessor to the private constructor.</summary>
    private readonly ConstructorInfo Ctor;

    /// <summary>The <see cref="SvoBehavior"/> of the underlying value.</summary>
    private readonly SvoBehavior Behavior;

    /// <summary>Creates a new instance of the <see cref="SvoTypeConverter"/> class.</summary>
    /// <param name="type">
    /// The type to convert for.
    /// </param>
    public SvoTypeConverter(Type type)
    {
        Guard.NotNull(type, nameof(type));

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Svo<>) && type.GetGenericArguments().Length == 1)
        {
            m_Value = Not.Null(type.GetField(nameof(m_Value), NonPublicInstance));
            var ctors = type.GetConstructors(NonPublicInstance);
            Ctor = ctors.First(ctor => ctor.GetParameters().Length == 1);
            Behavior = Not.Null((SvoBehavior?)Activator.CreateInstance(type.GetGenericArguments()[0]));
        }
        else throw new ArgumentException("Incompatible type", nameof(type));
    }

    private const BindingFlags NonPublicInstance = (BindingFlags)36;

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => Behavior.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    [Pure]
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => Behavior.CanConvertTo(context, destinationType);

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
            var id = Behavior.ConvertFrom(context, culture, value);
            return Ctor.Invoke(new[] { id });
        }
    }

    /// <inheritdoc />
    [Pure]
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        var str = m_Value.GetValue(value);
        return Behavior.ConvertTo(context, culture, str, destinationType);
    }
}
