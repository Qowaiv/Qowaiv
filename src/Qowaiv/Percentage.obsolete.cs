using System;
using System.Globalization;

namespace Qowaiv
{
    public partial struct Percentage
    {
        /// <summary>The percentage symbol (%).</summary>
        [Obsolete("Use PercentSymbol instead. Will be dropped when the next major version is released.")]
        public static readonly string PercentageMark = "%";
        /// <summary>The per mille symbol (‰).</summary>
        [Obsolete("Use PerMilleSymbol instead. Will be dropped when the next major version is released.")]
        public static readonly string PerMilleMark = "‰";
        /// <summary>The per ten thousand symbol (0/000).</summary>
        [Obsolete("Use PerTenThousandSymbol instead. Will be dropped when the next major version is released.")]
        public static readonly string PerTenThousandMark = "‱";

        /// <summary>Returns a <see cref="string"/> that represents the current Percentage formatted with a per ten Thousand mark.</summary>
        [Obsolete(@"Use ToString(""PT"", CultureInfo.InvariantCulture) instead. Will be dropped when the next major version is released.")]
        public string ToPerTenThousandMarkString() => ToString(PerTenThousendFormat, CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current Percentage formatted with a per mille mark.</summary>
        [Obsolete(@"Use ToString(""PM"", CultureInfo.InvariantCulture) instead. Will be dropped when the next major version is released.")]
        public string ToPerMilleString() => ToString(PerMilleFormat, CultureInfo.InvariantCulture);
    }
}
