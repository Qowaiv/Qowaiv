using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Qowaiv.Specs.TestTools;
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class RandomFractionAttribute(int count, bool includeZero = false) : NUnitAttribute, IParameterDataSource
{
    public int Count { get; } = count;

    public bool IncludeZero { get; } = includeZero;

    /// <inheritdoc />
    [Pure]
    public IEnumerable GetData(IParameterInfo parameter)
    {
        var rnd = Randomizer.GetRandomizer(parameter.ParameterInfo);

        return Enumerable.Range(0, Count).Select(_ => Fraction(rnd));

        Fraction Fraction(Randomizer rnd)
        {
            var num = rnd.Next(-255, 255) * rnd.Next(255);
            while (num == 0 && !IncludeZero)
            {
                num = rnd.Next(-255, 255) * rnd.Next(255);
            }
            var den = rnd.Next(1, 255) * rnd.Next(1, 255);
            return num.DividedBy(den);
        }
    }
}
