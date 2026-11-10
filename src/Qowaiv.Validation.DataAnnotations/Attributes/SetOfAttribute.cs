using System.Net.Http;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Base <see cref="ValidationAttribute"/> for allowing or forbidding a set of values.</summary>
/// <typeparam name="TValue">
/// The type of the value.
/// </typeparam>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[CLSCompliant(false)]
public abstract class SetOfAttribute<TValue>(params object[] values)
    : ValidationAttribute(() => QowaivValidationMessages.AllowedValuesAttribute_ValidationError)
{
    /// <summary>Specify a custom type converter to convert the values to convert.</summary>
    public Type? TypeConverter { get; init; }

    /// <summary>The result to return when the value of <see cref="IsValid(object)" />
    /// equals one of the values of the <see cref="SetOfAttribute{TValue}" />.
    /// </summary>
    protected abstract bool OnEqual { get; }

    /// <summary>Gets the values.</summary>
    public IReadOnlyCollection<TValue> Values => Set;

    /// <summary>Returns true if the value is allowed.</summary>
    [Pure]
    public sealed override bool IsValid(object? value)
        => value is null
        || OnEqual == Set.Contains((TValue)value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private HashSet<TValue> Set => field ??= Init();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly object[] Raw = values;

    [Pure]
    private HashSet<TValue> Init()
    {
        var all = new HashSet<TValue>();
        var converter = Converter();

        foreach (var value in Raw)
        {
            if (value is TValue typed)
            {
                all.Add(typed);
            }
            else if (converter.ConvertFromCultureInvariant<TValue>(value) is TValue converted)
            {
                all.Add(converted);
            }
        }
        return all;
    }

    /// <summary>Resolves the Type converter to use.</summary>
    /// <remarks>
    /// Because .NET doesn't provide a built-in converter for <see cref="HttpMethod"/>, we do.
    /// </remarks>
    [Pure]
    private TypeConverter Converter() => TypeConverter switch
    {
        not null => Guard.IsInstanceOf<TypeConverter>(Activator.CreateInstance(TypeConverter)),
        _ when typeof(TValue) == typeof(HttpMethod) => new Conversion.Web.HttpMethodTypeConverter(),
        _ => TypeDescriptor.GetConverter(typeof(TValue)),
    };
}
