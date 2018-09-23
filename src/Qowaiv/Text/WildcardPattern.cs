using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Qowaiv.Text
{
    /// <summary>Represents a wildcard pattern.</summary>
    [Serializable, DebuggerDisplay("{DebuggerDisplay}")]
    public class WildcardPattern : ISerializable
    {
        /// <summary>Initializes a new instance of a wild card pattern.</summary>
        /// <remarks>
        /// No public constructor without arguments.
        /// </remarks>
        protected WildcardPattern()
        {
            SingleChar = '?';
            MultipleChars = '*';
            Pattern = "*";
        }

        /// <summary>Initializes a new instance of a wild card pattern.</summary>
        /// <param name="pattern">
        /// The pattern to match on.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// pattern is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// pattern is empty or invalid.
        /// </exception>
        public WildcardPattern(string pattern)
            : this(pattern, WildcardPatternOptions.None, StringComparison.CurrentCulture) { }

        /// <summary>Initializes a new instance of a wild card pattern.</summary>
        /// <param name="pattern">
        /// The pattern to match on.
        /// </param>
        /// <param name="options">
        /// The wildcard pattern options.
        /// </param>
        /// <param name="comparisonType">
        /// The type of comparison.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// pattern is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// pattern is empty or invalid.
        /// </exception>
        public WildcardPattern(string pattern, WildcardPatternOptions options, StringComparison comparisonType)
            : this()
        {
            Pattern = Guard.NotNullOrEmpty(pattern, nameof(pattern));

            if (options.HasFlag(WildcardPatternOptions.SqlWildcards))
            {
                if (pattern.Contains("%%")) { throw new ArgumentException(QowaivMessages.ArgumentException_InvalidWildcardPattern, "pattern"); }

                SingleChar = '_';
                MultipleChars = '%';
            }
            else if (pattern.Contains("**")) { throw new ArgumentException(QowaivMessages.ArgumentException_InvalidWildcardPattern, "pattern"); }

            Options = options;
            ComparisonType = comparisonType;
        }

        #region Serializable

        /// <summary>Initializes a new instance of a wild card pattern based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected WildcardPattern(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            Pattern = info.GetString("Pattern");
            Options = (WildcardPatternOptions)info.GetInt32("Options");
            ComparisonType = (StringComparison)info.GetInt32("ComparisonType");
        }

        /// <summary>Adds the underlying property of a wild card pattern to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) { GetObjectData(info, context); }

        /// <summary>Adds the underlying property of a wild card pattern to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        /// <remarks>
        /// this is used by ISerializable.GetObjectData() so that it can be
        /// changed by derived classes.
        /// </remarks>
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Pattern", Pattern);
            info.AddValue("Options", (int)Options);
            info.AddValue("ComparisonType", (int)ComparisonType);
        }

        #endregion

        /// <summary>The wildcard pattern options.</summary>
        public WildcardPatternOptions Options { get; private set; }
        /// <summary>The comparison type.</summary>
        public StringComparison ComparisonType { get; private set; }

        /// <summary>The wildcard pattern.</summary>
        protected string Pattern { get; private set; }
        /// <summary>The wildcard single char.</summary>
        protected char SingleChar { get; set; }
        /// <summary>The wildcard multiple chars.</summary>
        protected char MultipleChars { get; set; }

        /// <summary>Returns true if matching is culture independent, otherwise false.</summary>
        protected bool IsCultureIndependent
        {
            get
            {
                return 
                    // The first test is never hit (yet) because we only need
                    // this when ignore case.
                    //ComparisonType == StringComparison.InvariantCulture || 
                    ComparisonType == StringComparison.InvariantCultureIgnoreCase;
            }
        }

        /// <summary>Returns true if the case should be ignored, otherwise false.</summary>
        protected bool IgnoreCase
        {
            get
            {
                return
                    ComparisonType == StringComparison.CurrentCultureIgnoreCase ||
                    ComparisonType == StringComparison.InvariantCultureIgnoreCase ||
                    ComparisonType == StringComparison.OrdinalIgnoreCase;
            }
        }

        /// <summary>Indicates whether the wildcard pattern finds a match in the specified input string.</summary>
        /// <param name="input">
        /// The string to search for a match.
        /// </param>
        /// <returns>
        /// true if the wildcard pattern finds a match; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// input is null.
        /// </exception>
        public bool IsMatch(string input)
        {
            Guard.NotNull(input, nameof(input));
            return IsMatch(input, 0, 0);
        }
        /// <summary>Handles the actual matching.</summary>
        /// <param name="input">
        /// The input to match.
        /// </param>
        /// <param name="p">
        /// the current position of the pattern.
        /// </param>
        /// <param name="i">
        /// the current position of the input.
        /// </param>
        /// <returns></returns>
        private bool IsMatch(string input, int p, int i)
        {
            // At least on of the indexes is at the end.
            if (Pattern.Length == p || input.Length == i)
            {
                // If we reach at the end of both strings, we are done.
                if (Pattern.Length == p && input.Length == i)
                {
                    return true;
                }
                // If we are the end of input, this is valid only when the pattern has a '*'
                // and that is also the last character.
                return  input.Length == i && Pattern[p] == MultipleChars && p == Pattern.Length - 1;
            }

            var chP = Pattern[p];
            var chI = input[i];

            // If the first string contains '?'.
            if (chP == SingleChar)
            {
                // First test trailing, then non trailing.
                if(Options.HasFlag(WildcardPatternOptions.SingleOrTrailing))
                {
                    return IsMatch(input, p + 1, i) || IsMatch(input, p + 1, i + 1);
                }
                return IsMatch(input, p + 1, i + 1) ;
            }
            
            // If there is *, then there are two possibilities:
            // - We consider current character of second string.
            // - We ignore current character of second string.
            if (chP == MultipleChars)
            {
                return IsMatch(input, p + 1, i) || IsMatch(input, p, i + 1);
            }

            // If the current characters of both strings match.
            if( Equals(chP,chI))
            {
                return IsMatch(input, p + 1, i + 1);
            }

            // No match, exit.
            return false;
        }
        private bool Equals(char l, char r)
        {
            if (l == r) { return true; }

            if (IgnoreCase)
            {
                var culture = IsCultureIndependent ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;
                var ll = Char.ToLower(l, culture);
                var rl = Char.ToLower(r, culture);
                return ll == rl;
            }
            return false;
        }

        /// <summary>Indicates whether the specified wildcard pattern finds a match in the specified input string.</summary>
        /// <param name="pattern">
        /// The string that represents the wildcard pattern.
        /// </param>
        /// <param name="input">
        /// The string to search for a match.
        /// </param>
        /// <returns>
        /// true if the wildcard pattern finds a match; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// input is null.
        /// </exception>
        public static bool IsMatch(string pattern, string input)
        {
            return IsMatch(pattern, input, WildcardPatternOptions.None, StringComparison.CurrentCulture);
        }
        /// <summary>Indicates whether the specified wildcard pattern finds a match in the specified input string.</summary>
        /// <param name="pattern">
        /// The string that represents the wildcard pattern.
        /// </param>
        /// <param name="input">
        /// The string to search for a match.
        /// </param>
        /// <param name="options">
        /// The wildcard pattern options.
        /// </param>
        /// <param name="comparisonType">
        /// The type of comparison.
        /// </param>
        /// <returns>
        /// true if the wildcard pattern finds a match; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// input is null.
        /// </exception>
        public static bool IsMatch(string pattern, string input, WildcardPatternOptions options, StringComparison comparisonType)
        {
            var wildcard = new WildcardPattern(pattern, options, comparisonType);
            return wildcard.IsMatch(input);
        }

        /// <summary>Represents the wildcard pattern as debug string.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append('{').Append(Pattern).Append('}');
                if (Options != WildcardPatternOptions.None)
                {
                    sb.Append(", ").Append(Options);
                }
                if (ComparisonType != StringComparison.CurrentCulture)
                {
                    sb.Append(", ").Append(ComparisonType);
                }
                return sb.ToString();
            }
        }
    }
}
