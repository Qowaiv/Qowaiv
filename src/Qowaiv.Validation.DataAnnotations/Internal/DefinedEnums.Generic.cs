using System.Collections.Frozen;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Generic variant of <see cref="DefinedEnums"/>, that does not box the values to validate.</summary>
internal abstract class DefinedEnums<TEnum> : DefinedEnums
    where TEnum : struct, Enum
{
    [Pure]
    public static DefinedEnums<TEnum> New(bool onlyAllowDefinedFlags)
        => onlyAllowDefinedFlags
        || !typeof(TEnum).GetCustomAttributes<FlagsAttribute>().Any()
        ? new Frozen()
        : new Flags();

    [Pure]
    public sealed override bool Contains(object value) => Contains((TEnum)value);

    [Pure]
    public abstract bool Contains(TEnum value);

    [Pure]
    private static IEnumerable<TEnum> Defined() => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

    [DebuggerDisplay("Type = {typeof(TEnum)}, Count = {Values.Count}")]
    private sealed class Frozen : DefinedEnums<TEnum>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly FrozenSet<TEnum> Values = Defined().ToFrozenSet();

        [Pure]
        public override bool Contains(TEnum value) => Values.Contains(value);
    }

    [DebuggerDisplay("Type = {typeof(TEnum)}, Mask = {Mask}")]
    private sealed class Flags : DefinedEnums<TEnum>
    {
        private static readonly Func<TEnum, ulong> ToBits = CreateConverter();

        private readonly ulong Mask = Defined().Aggregate(0UL, (mask, value) => mask | ToBits(value));

        [Pure]
        public override bool Contains(TEnum value)
        {
            var bits = ToBits(value);
            return (Mask & bits) == bits;
        }

        [Pure]
        private static Func<TEnum, ulong> CreateConverter()
        {
            var par = Parameter(typeof(TEnum));
            var underlying = Convert(par, Enum.GetUnderlyingType(typeof(TEnum)));
            var bits = Convert(underlying, typeof(ulong));
            return Lambda<Func<TEnum, ulong>>(bits, par).Compile();
        }
    }
}
