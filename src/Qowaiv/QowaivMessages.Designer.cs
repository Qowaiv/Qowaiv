//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Qowaiv {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class QowaivMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QowaivMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Qowaiv.QowaivMessages", typeof(QowaivMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The wildcard pattern is invalid..
        /// </summary>
        public static string ArgumentException_InvalidWildcardPattern {
            get {
                return ResourceManager.GetString("ArgumentException_InvalidWildcardPattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument must be {0}.
        /// </summary>
        public static string ArgumentException_Must {
            get {
                return ResourceManager.GetString("ArgumentException_Must", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The argument must implement System.IFormattable..
        /// </summary>
        public static string ArgumentException_NotIFormattable {
            get {
                return ResourceManager.GetString("ArgumentException_NotIFormattable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The byte array should have size of 8..
        /// </summary>
        public static string ArgumentException_TimestampArrayShouldHaveSize8 {
            get {
                return ResourceManager.GetString("ArgumentException_TimestampArrayShouldHaveSize8", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decimals can only round to between -28 and 28 digits of precision..
        /// </summary>
        public static string ArgumentOutOfRange_DecimalRound {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_DecimalRound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number can not represent an Elo..
        /// </summary>
        public static string ArgumentOutOfRange_Elo {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_Elo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The allowed error when converting a Decimal to a Fraction should be between Fraction.Epsilon and 1..
        /// </summary>
        public static string ArgumentOutOfRange_FractionError {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_FractionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number style &apos;{0}&apos; is not supported..
        /// </summary>
        public static string ArgumentOutOfRange_NumberStyleNotSupported {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_NumberStyleNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value is either too large or too small for a percentage..
        /// </summary>
        public static string ArgumentOutOfRange_Percentage {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_Percentage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Percentages can only round to between -26 and 26 digits of precision..
        /// </summary>
        public static string ArgumentOutOfRange_PercentageRound {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_PercentageRound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Year, and Month parameters describe an un-representable year-month..
        /// </summary>
        public static string ArgumentOutOfRange_YearMonth {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_YearMonth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding a date span only supports &apos;Default&apos; and &apos;DaysFirst&apos;..
        /// </summary>
        public static string ArgumentOutOfRangeException_AddDateSpan {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_AddDateSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified years, months and days results in an un-representable DateSpan..
        /// </summary>
        public static string ArgumentOutOfRangeException_DateSpan {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_DateSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} operation could not be applied. There is a mismatch between {1} and {2}..
        /// </summary>
        public static string CurrencyMismatchException {
            get {
                return ResourceManager.GetString("CurrencyMismatchException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Index (zero based) must be greater than or equal to zero and less than the size of the argument list..
        /// </summary>
        public static string FormatException_IndexOutOfRange {
            get {
                return ResourceManager.GetString("FormatException_IndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input string was not in a correct format..
        /// </summary>
        public static string FormatException_InvalidFormat {
            get {
                return ResourceManager.GetString("FormatException_InvalidFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid Base32 string.
        /// </summary>
        public static string FormatExceptionBase32 {
            get {
                return ResourceManager.GetString("FormatExceptionBase32", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid BIC.
        /// </summary>
        public static string FormatExceptionBusinessIdentifierCode {
            get {
                return ResourceManager.GetString("FormatExceptionBusinessIdentifierCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid CAS Registry Number.
        /// </summary>
        public static string FormatExceptionCasRegistryNumber {
            get {
                return ResourceManager.GetString("FormatExceptionCasRegistryNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid country.
        /// </summary>
        public static string FormatExceptionCountry {
            get {
                return ResourceManager.GetString("FormatExceptionCountry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid cryptographic seed.
        /// </summary>
        public static string FormatExceptionCryptographicSeed {
            get {
                return ResourceManager.GetString("FormatExceptionCryptographicSeed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid currency.
        /// </summary>
        public static string FormatExceptionCurrency {
            get {
                return ResourceManager.GetString("FormatExceptionCurrency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid date.
        /// </summary>
        public static string FormatExceptionDate {
            get {
                return ResourceManager.GetString("FormatExceptionDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid date span.
        /// </summary>
        public static string FormatExceptionDateSpan {
            get {
                return ResourceManager.GetString("FormatExceptionDateSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid Elo.
        /// </summary>
        public static string FormatExceptionElo {
            get {
                return ResourceManager.GetString("FormatExceptionElo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid email address.
        /// </summary>
        public static string FormatExceptionEmailAddress {
            get {
                return ResourceManager.GetString("FormatExceptionEmailAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not valid email addresses.
        /// </summary>
        public static string FormatExceptionEmailAddressCollection {
            get {
                return ResourceManager.GetString("FormatExceptionEmailAddressCollection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid energy label.
        /// </summary>
        public static string FormatExceptionEnergyLabel {
            get {
                return ResourceManager.GetString("FormatExceptionEnergyLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid amount.
        /// </summary>
        public static string FormatExceptionFinancialAmount {
            get {
                return ResourceManager.GetString("FormatExceptionFinancialAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid fraction.
        /// </summary>
        public static string FormatExceptionFraction {
            get {
                return ResourceManager.GetString("FormatExceptionFraction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid gender.
        /// </summary>
        public static string FormatExceptionGender {
            get {
                return ResourceManager.GetString("FormatExceptionGender", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid house number.
        /// </summary>
        public static string FormatExceptionHouseNumber {
            get {
                return ResourceManager.GetString("FormatExceptionHouseNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid identifier.
        /// </summary>
        public static string FormatExceptionIdentifier {
            get {
                return ResourceManager.GetString("FormatExceptionIdentifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid IBAN.
        /// </summary>
        public static string FormatExceptionInternationalBankAccountNumber {
            get {
                return ResourceManager.GetString("FormatExceptionInternationalBankAccountNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid internet media type.
        /// </summary>
        public static string FormatExceptionInternetMediaType {
            get {
                return ResourceManager.GetString("FormatExceptionInternetMediaType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not valid date.
        /// </summary>
        public static string FormatExceptionLocalDateTime {
            get {
                return ResourceManager.GetString("FormatExceptionLocalDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid amount.
        /// </summary>
        public static string FormatExceptionMoney {
            get {
                return ResourceManager.GetString("FormatExceptionMoney", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid month.
        /// </summary>
        public static string FormatExceptionMonth {
            get {
                return ResourceManager.GetString("FormatExceptionMonth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid month span.
        /// </summary>
        public static string FormatExceptionMonthSpan {
            get {
                return ResourceManager.GetString("FormatExceptionMonthSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid percentage.
        /// </summary>
        public static string FormatExceptionPercentage {
            get {
                return ResourceManager.GetString("FormatExceptionPercentage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid postal code.
        /// </summary>
        public static string FormatExceptionPostalCode {
            get {
                return ResourceManager.GetString("FormatExceptionPostalCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid GUID.
        /// </summary>
        public static string FormatExceptionQGuid {
            get {
                return ResourceManager.GetString("FormatExceptionQGuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid sex.
        /// </summary>
        public static string FormatExceptionSex {
            get {
                return ResourceManager.GetString("FormatExceptionSex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid stream size.
        /// </summary>
        public static string FormatExceptionStreamSize {
            get {
                return ResourceManager.GetString("FormatExceptionStreamSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid SQL timestamp.
        /// </summary>
        public static string FormatExceptionTimestamp {
            get {
                return ResourceManager.GetString("FormatExceptionTimestamp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid GUID.
        /// </summary>
        public static string FormatExceptionUuid {
            get {
                return ResourceManager.GetString("FormatExceptionUuid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid week date.
        /// </summary>
        public static string FormatExceptionWeekDate {
            get {
                return ResourceManager.GetString("FormatExceptionWeekDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid year.
        /// </summary>
        public static string FormatExceptionYear {
            get {
                return ResourceManager.GetString("FormatExceptionYear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid year-month..
        /// </summary>
        public static string FormatExceptionYearMonth {
            get {
                return ResourceManager.GetString("FormatExceptionYearMonth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid yes-no value.
        /// </summary>
        public static string FormatExceptionYesNo {
            get {
                return ResourceManager.GetString("FormatExceptionYesNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hashing is not supported by {0}..
        /// </summary>
        public static string HashingNotSupported {
            get {
                return ResourceManager.GetString("HashingNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cast from {0} to {1} is not valid..
        /// </summary>
        public static string InvalidCastException_FromTo {
            get {
                return ResourceManager.GetString("InvalidCastException_FromTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sequential UUID can only be generated between 1970-01-01 and 9276-12-03..
        /// </summary>
        public static string InvalidOperation_SequentialUUID {
            get {
                return ResourceManager.GetString("InvalidOperation_SequentialUUID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sequence contains no elements..
        /// </summary>
        public static string InvalidOperationException_NoElements {
            get {
                return ResourceManager.GetString("InvalidOperationException_NoElements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An not set email address can not be shown with a display name..
        /// </summary>
        public static string InvalidOperationException_WithDisplayName {
            get {
                return ResourceManager.GetString("InvalidOperationException_WithDisplayName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON deserialization from a date is not supported..
        /// </summary>
        public static string JsonSerialization_DateTimeNotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_DateTimeNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON deserialization from a number is not supported..
        /// </summary>
        public static string JsonSerialization_DoubleNotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_DoubleNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input &apos;{0}&apos; not supported. Expects &apos;{1}&apos;..
        /// </summary>
        public static string JsonSerialization_InputNotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_InputNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON deserialization from an integer is not supported..
        /// </summary>
        public static string JsonSerialization_Int64NotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_Int64NotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON deserialization from null is not supported..
        /// </summary>
        public static string JsonSerialization_NullNotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_NullNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Result type &apos;{0}&apos; not supported. Expects &apos;{1}&apos;..
        /// </summary>
        public static string JsonSerialization_ResultTypeNotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_ResultTypeNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected token parsing {0}. {1} is not supported..
        /// </summary>
        public static string JsonSerialization_TokenNotSupported {
            get {
                return ResourceManager.GetString("JsonSerialization_TokenNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Converter can not convert from System.String..
        /// </summary>
        public static string NotSupportedException_ConverterCanNotConvertFromString {
            get {
                return ResourceManager.GetString("NotSupportedException_ConverterCanNotConvertFromString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type must be a none generic type..
        /// </summary>
        public static string NotSupportedException_NoGenericType {
            get {
                return ResourceManager.GetString("NotSupportedException_NoGenericType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The country &apos;{0} ({1})&apos; is not supported as region info..
        /// </summary>
        public static string NotSupportedExceptionCountryToRegionInfo {
            get {
                return ResourceManager.GetString("NotSupportedExceptionCountryToRegionInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DateSpan overflowed because the resulting duration is too long..
        /// </summary>
        public static string OverflowException_DateSpan {
            get {
                return ResourceManager.GetString("OverflowException_DateSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decimal overflowed while rounding..
        /// </summary>
        public static string OverflowException_DecimalRound {
            get {
                return ResourceManager.GetString("OverflowException_DecimalRound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value was either too large or too small for a Fraction..
        /// </summary>
        public static string OverflowException_Fraction {
            get {
                return ResourceManager.GetString("OverflowException_Fraction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not parse &apos;{0}&apos; for {1}..
        /// </summary>
        public static string Unparsable {
            get {
                return ResourceManager.GetString("Unparsable", resourceCulture);
            }
        }
    }
}
