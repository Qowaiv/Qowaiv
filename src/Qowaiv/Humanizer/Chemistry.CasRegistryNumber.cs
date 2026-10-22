using Qowaiv.Chemistry;

namespace Qowaiv.Humanizer;

public static partial class Humanize
{
    extension(int number)
    {
        /// <summary>Interprets the <see cref="int" /> if it was written as a CAS Registry Number.</summary>
        [Pure]
        public CasRegistryNumber CasNr() => CasRegistryNumber.Create(number);
    }

    extension(long number)
    {
        /// <summary>Interprets the <see cref="long" /> if it was written as a CAS Registry Number.</summary>
        [Pure]
        public CasRegistryNumber CasNr() => CasRegistryNumber.Create(number);
    }
}
