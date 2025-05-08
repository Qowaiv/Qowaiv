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
    }

    internal readonly struct NoneFormattable : IEquatable<NoneFormattable>
    {
        private readonly int Value;

        public NoneFormattable(int value) => Value = value;

        public bool Equals(NoneFormattable other) => true;

        public override string ToString() => nameof(NoneFormattable);
    }
}


