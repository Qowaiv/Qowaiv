using Qowaiv.Customization;

namespace Customization.CustomBehavior_specs;

public class ToString
{
    [Test]
    public void supports_none_formattable_raw_types()
    {
        var behavior = new NoneFormattableBehavior();
        behavior.ToString(new(42), "", CultureInfo.InvariantCulture).Should().Be("NoneFormattable");
    }

    internal sealed class NoneFormattableBehavior : CustomBehavior<NoneFormattable>
    {
        public override bool TryTransform(string? str, IFormatProvider? formatProvider, [NotNullWhen(true)] out NoneFormattable transformed)
        {
            transformed = new(42);
            return true;
        }

        public override bool TryTransform(NoneFormattable value, out NoneFormattable transformed)
        {
            transformed = value;
            return true;
        }
    }

    internal readonly struct NoneFormattable(int value) : IEquatable<NoneFormattable>
    {
       public bool Equals(NoneFormattable other) => true;

        public override string ToString() => value.ToString();
    }
}


