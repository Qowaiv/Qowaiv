using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Qowaiv.Validation.DataAnnotations.Reflection;

internal static class ValidationAttributes
{
    public delegate ValidationResult? Contract(ValidationAttribute attr, object? value, ValidationContext context);

    public static readonly Contract IsValid = Compile();

    [Pure]
    private static Contract Compile()
    {
        var att = Parameter(typeof(ValidationAttribute), "attr");
        var mth = typeof(ValidationAttribute).GetMethod(nameof(IsValid), Instance)!;
        var val = Parameter(typeof(object), "value");
        var ctx = Parameter(typeof(ValidationContext), "context");
        var body = Call(att, mth, val, ctx);
        var expr = Lambda<Contract>(body, att, val, ctx);
        return expr.Compile();
    }

#pragma warning disable S3011 // Reflection should not be used to increase accessibility.
    public const BindingFlags Instance = BindingFlags.Instance | BindingFlags.NonPublic;
}
