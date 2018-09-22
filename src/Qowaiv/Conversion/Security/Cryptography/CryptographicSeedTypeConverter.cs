using Qowaiv.Security.Cryptography;
using System.Globalization;

namespace Qowaiv.Conversion.Security.Cryptography
{
    /// <summary>Provides a conversion for a cryptographic seed.</summary>
    public class CryptographicSeedTypeConverter : SvoTypeConverter<CryptographicSeed>
    {
        /// <inheritdoc/>
        protected override CryptographicSeed FromString(string str, CultureInfo culture) => CryptographicSeed.Parse(str);
    }
}
