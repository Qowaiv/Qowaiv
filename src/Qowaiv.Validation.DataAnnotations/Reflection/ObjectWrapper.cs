using System.Collections.Enumerable;
using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Qowaiv.Validation.DataAnnotations.Reflection;

internal static class ObjectWrapper
{
    extension(object obj)
    {
        /// <summary>Tries to resolve the value of the length property.</summary>
        [Pure]
        public long? TryLength() => obj.Try("Length");

        /// <summary>Tries to resolve the value of the count property.</summary>
        [Pure]
        public long? TryCount() => obj.Try("Count");

        [Pure]
        private long? Try(string name) => obj switch
        {
            string str => str.Length,
            ICollection collection => collection.Count,
            _ when Gets.GetOrAdd(new(obj.GetType(), name), Compile) is { } size => size(obj),
            IEnumerable enumerable => enumerable.Count(),
            _ => null,
        };
    }

    [Pure]
    private static Func<object, long>? Compile(Prop prop)
    {
        if (prop.DeclaringType.GetRuntimeProperty(prop.Name) is not { IsSupported: true } info)
            return null;

        var obj = Parameter(typeof(object), "obj");
        var val = (Expression)Property(Convert(obj, prop.DeclaringType!), info);
        if (info.PropertyType != typeof(long))
        {
            val = Convert(val, typeof(long));
        }
        return Lambda<Func<object, long>>(val, obj).Compile();
    }

    private readonly record struct Prop(Type DeclaringType, string Name);

    private static readonly ConcurrentDictionary<Prop, Func<object, long>?> Gets = new();

    extension(PropertyInfo prop)
    {
        public bool IsSupported
            => prop.CanRead
            && (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long));
    }
}
