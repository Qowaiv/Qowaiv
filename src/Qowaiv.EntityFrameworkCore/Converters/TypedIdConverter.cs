using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Qowaiv.EntityFrameworkCore.Converters;

public static class TypedIdConverter
{
    [Pure]
    public static ValueConverter Create<TId>() where TId: struct, INext<TId>
        => Create(typeof(TId));

    [Pure]
    public static ValueConverter Create(Type type)
    {
        Guard.NotNull(type);

        var converter = TypeDescriptor.GetConverter(type);

        if (converter.GetType() == typeof(TypeConverter))
            throw new NotSupportedException($"Type '{type.ToCSharpString()}' lacks a custom type converter.");

        var underlying = type.GetField("m_Value", PrivateInstance)?.FieldType is { } @private && Underlying.Contains(@private)
            ? @private
            : typeof(string);

        Func<object, object> converToProvider = id => converter.ConvertTo(id, underlying);
        Func<object, object> convertFromProvider = provider => converter.ConvertFrom(provider);

        throw new NotImplementedException();
    }

    private static readonly FrozenSet<Type> Underlying = [typeof(Guid), typeof(long), typeof(int), typeof(string)];

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    // We're doing reflection on code we know, but must be private
    private const BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;

    
}
