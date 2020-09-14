using Qowaiv.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Qowaiv.Globalization
{
    /// <summary>Represents country specific postal code information.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public sealed partial class PostalCodeCountryInfo
    {
        /// <summary>Constructor.</summary>
        private PostalCodeCountryInfo() { }

        /// <summary>Gets the country.</summary>
        public Country Country { get; private set; }

        #region Properties

        /// <summary>Returns true if the country has a postal code system, otherwise false.</summary>
        public bool HasPostalCode { get { return this.ValidationPattern != null; } }

        /// <summary>Returns true if the country has supports formatting, otherwise false.</summary>
        public bool HasFormatting { get { return this.FormattingSearchPattern != null; } }

        /// <summary>Returns true if the country has only one postal code, otherwise false.</summary>
        public bool IsSingleValue { get; private set; }

        /// <summary>Gets the postal code validation pattern for the country.</summary>
        private Regex ValidationPattern { get; set; }
        
        /// <summary>Gets the postal code formatting search pattern for the country.</summary>
        private Regex FormattingSearchPattern { get; set; }
        
        /// <summary>Gets the postal code formatting replace pattern for the country.</summary>
        private string FormattingReplacePattern { get; set; }

        #endregion

        #region Methods

        /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
        /// <param name="postalcode">
        /// The postal code to test.
        /// </param>
        /// <remarks>
        /// Returns false if the country does not have postal codes.
        /// </remarks>
        public bool IsValid(string postalcode)
        {
            return
                !string.IsNullOrEmpty(postalcode) &&
                HasPostalCode &&
                postalcode.Buffer().Unify().Matches(ValidationPattern);
        }

        /// <summary>Formats the postal code.</summary>
        /// <param name="postalcode">
        /// The postal code.
        /// </param>
        /// <returns>
        /// A formatted string representing the postal code.
        /// </returns>
        /// <remarks>
        /// If the country supports formatting and if the postal code is valid
        /// for the country.
        /// </remarks>
        public string Format(string postalcode)
        {
            if (HasFormatting && IsValid(postalcode))
            {
                return FormattingSearchPattern.Replace(postalcode, FormattingReplacePattern);
            }
            if (Unknown.IsUnknown(postalcode, CultureInfo.InvariantCulture))
            {
                return "?";
            }
            return postalcode ?? string.Empty;
        }

        /// <summary>Gets the single value if supported, otherwise string.Empty.</summary>
        public string GetSingleValue()
        {
            return this.IsSingleValue ? this.FormattingReplacePattern : string.Empty;
        }

        /// <summary>Returns a <see cref="string"/> that represents the current postal code country info for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Postal code[{0}], ", Country.IsoAlpha2Code);
                if (!this.HasPostalCode)
                {
                    sb.Append("none");
                }
                else
                {
                    if (this.IsSingleValue)
                    {
                        sb.Append("Value: ").Append(this.FormattingReplacePattern);
                    }
                    else
                    {
                        sb.Append("Pattern: ")
                        .Append(this.ValidationPattern);

                        if (this.HasFormatting)
                        {
                            sb.Append(", Format: ")
                            .Append(this.FormattingReplacePattern);
                        }
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>Gets countries without a postal code system.</summary>
        public static IEnumerable<Country> GetCountriesWithoutPostalCode()
        {
            return Country.GetExisting().Where(country => !GetInstance(country).HasPostalCode);
        }

        /// <summary>Gets countries with postal codes with formatting.</summary>
        public static IEnumerable<Country> GetCountriesWithFormatting()
        {
            return Country.GetExisting().Where(country => GetInstance(country).HasFormatting);
        }

        /// <summary>Gets countries with a single postal code value.</summary>
        public static IEnumerable<Country> GetCountriesWithSingleValue()
        {
            return Country.GetExisting().Where(country => GetInstance(country).IsSingleValue);
        }

        #endregion

        #region Factory methods

        /// <summary>Gets the postal code country info associated with the specified country.</summary>
        /// <param name="country">
        /// The specified country.
        /// </param>
        public static PostalCodeCountryInfo GetInstance(Country country)
        {
            if (Instances.TryGetValue(country, out PostalCodeCountryInfo instance))
            {
                return instance;
            }
            return new PostalCodeCountryInfo
            {
                Country = country,
            };
        }

        /// <summary>Creates a new instance.</summary>
        /// <remarks>
        /// Used for initializing the Instances dictionary.
        /// </remarks>
        private static PostalCodeCountryInfo New(Country country, string validation, string search = null, string replace = null, bool isSingle = false)
        {
            return new PostalCodeCountryInfo
            {
                Country = country,
                ValidationPattern = new Regex(validation, RegexOptions.Compiled | RegexOptions.IgnoreCase),
                FormattingSearchPattern = string.IsNullOrEmpty(search) ? null : new Regex(search, RegexOptions.Compiled),
                FormattingReplacePattern = replace,
                IsSingleValue = isSingle
            };
        }

        #endregion
    }
}
