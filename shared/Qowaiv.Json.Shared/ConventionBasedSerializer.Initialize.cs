using System.Linq.Expressions;
using System.Reflection;

namespace Qowaiv.Internals;

internal sealed partial class ConventionBasedSerializer<TSvo>
{
    private Type SvoType { get; } = typeof(TSvo).NotNullable;

    private void Initialize()
    {
        var factories = SvoType
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(IsFactory);

        foreach (var factory in factories)
        {
            var parameterType = factory.GetParameters()[0].ParameterType;

            if (parameterType == typeof(string))
            {
                fromJsonString = CompileDeserialize<string?>(factory);
            }
            else if (parameterType == typeof(double))
            {
                fromJsonDouble = CompileDeserialize<double>(factory);
            }
            else if (parameterType == typeof(long))
            {
                fromJsonLong = CompileDeserialize<long>(factory);
            }
            else if (parameterType == typeof(bool))
            {
                fromJsonBool = CompileDeserialize<bool>(factory);
            }
        }

        if (fromJsonString is { })
        {
            fromJsonDouble ??= (num) => fromJsonString(num.ToString(CultureInfo.InvariantCulture));
            fromJsonLong ??= (num) => fromJsonString(num.ToString(CultureInfo.InvariantCulture));
            fromJsonBool ??= (b) => fromJsonString(b ? "true" : "false");

            var toJson = SvoType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(IsToJson);

            toJsonObject = CompileSerialize(toJson);
        }
    }

    [Pure]
    private bool IsFactory(MethodInfo method)
        => method.ReturnType == SvoType
        && method.Name == nameof(FromJson)
        && method.GetParameters() is [var param]
        && IsSuportedNodeType(param.ParameterType);

    [Pure]
    private static bool IsToJson(MethodInfo method)
        => method.Name == nameof(ToJson)
        && method.GetParameters().Length is 0
        && method.ReturnType != typeof(void);

    [Pure]
    private Func<TSvo, object> CompileSerialize(MethodInfo? method)
    {
        var toJson = method ?? typeof(object).GetMethod(nameof(ToString))!;
        var svo = Expression.Parameter(typeof(TSvo), "svo");
        var par = SvoType == typeof(TSvo)
            ? (Expression)svo
            : Expression.Convert(svo, SvoType);

        Expression body = Expression.Call(par, toJson);

        // if nullable, add a convert.
        if (toJson.ReturnType != typeof(object))
        {
            body = Expression.Convert(body, typeof(object));
        }

        var expression = Expression.Lambda<Func<TSvo, object>>(body, svo);
        return expression.Compile();
    }

    [Pure]
    private static Func<TNode, TSvo> CompileDeserialize<TNode>(MethodInfo method)
    {
        var node = Expression.Parameter(typeof(TNode), "node");
        Expression body = Expression.Call(method, node);

        // If nullable, add a convert.
        if (method.ReturnType != typeof(TSvo))
        {
            body = Expression.Convert(body, typeof(TSvo));
        }

        var expression = Expression.Lambda<Func<TNode, TSvo>>(body, node);
        return expression.Compile();
    }

    [Pure]
    private static bool IsSuportedNodeType(Type type)
        => type == typeof(string)
        || type == typeof(double)
        || type == typeof(long)
        || type == typeof(bool);
}
