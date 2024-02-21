namespace Qowaiv.Text;

/// <summary>Represents a wildcard pattern.</summary>
[Serializable]
[DebuggerDisplay("{DebuggerDisplay}")]
public class WildcardPattern : ISerializable
{
    /// <summary>Initializes a new instance of the <see cref="WildcardPattern"/> class.</summary>
    /// <remarks>
    /// No public constructor without arguments.
    /// </remarks>
    protected WildcardPattern()
    {
        SingleChar = '?';
        MultipleChars = '*';
        Pattern = "*";
    }

    /// <summary>Initializes a new instance of the <see cref="WildcardPattern"/> class.</summary>
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

    /// <summary>Initializes a new instance of the <see cref="WildcardPattern"/> class.</summary>
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
        Pattern = GuardPattern(pattern, options);
        Options = options;
        ComparisonType = comparisonType;

        if (Options.HasFlag(WildcardPatternOptions.SqlWildcards))
        {
            SingleChar = '_';
            MultipleChars = '%';
        }
    }

    /// <summary>Initializes a new instance of the <see cref="WildcardPattern"/> class.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    protected WildcardPattern(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        Options = (WildcardPatternOptions)info.GetInt32("Options");
        Pattern = GuardPattern(info.GetString("Pattern"), Options);
        ComparisonType = (StringComparison)info.GetInt32("ComparisonType");

        if (Options.HasFlag(WildcardPatternOptions.SqlWildcards))
        {
            SingleChar = '_';
            MultipleChars = '%';
        }
    }

    private static string GuardPattern(string? pattern, WildcardPatternOptions options)
    {
        pattern = Guard.NotNullOrEmpty(pattern);

        if (options.HasFlag(WildcardPatternOptions.SqlWildcards))
        {
            if (pattern.Contains("%%"))
            {
                throw new ArgumentException(QowaivMessages.ArgumentException_InvalidWildcardPattern, nameof(pattern));
            }
        }
        else if (pattern.Contains("**"))
        {
            throw new ArgumentException(QowaivMessages.ArgumentException_InvalidWildcardPattern, nameof(pattern));
        }
        return pattern;
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
        Guard.NotNull(info);
        info.AddValue("Pattern", Pattern);
        info.AddValue("Options", (int)Options);
        info.AddValue("ComparisonType", (int)ComparisonType);
    }

    /// <summary>The wildcard pattern options.</summary>
    public WildcardPatternOptions Options { get; init; }

    /// <summary>The comparison type.</summary>
    public StringComparison ComparisonType { get; init; }

    /// <summary>The wildcard pattern.</summary>
    protected string Pattern { get; init; }

    /// <summary>The wildcard single char.</summary>
    protected char SingleChar { get; init; }

    /// <summary>The wildcard multiple chars.</summary>
    protected char MultipleChars { get; init; }

    /// <summary>Returns true if matching is culture independent, otherwise false.</summary>
    /// <remarks>
    /// The second test is never hit (yet) because we only need this when ignore case.
    /// </remarks>
    protected bool IsCultureIndependent
        => ComparisonType == StringComparison.InvariantCultureIgnoreCase;

    // || ComparisonType == StringComparison.InvariantCulture

    /// <summary>Returns true if the case should be ignored, otherwise false.</summary>
    protected bool IgnoreCase
        => ComparisonType == StringComparison.CurrentCultureIgnoreCase
        || ComparisonType == StringComparison.InvariantCultureIgnoreCase
        || ComparisonType == StringComparison.OrdinalIgnoreCase;

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
    [Pure]
    public bool IsMatch(string input)
    {
        Guard.NotNull(input);
        return Match(new(Pattern), new(input));
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
    [Pure]
    public static bool IsMatch(string pattern, string input)
        => IsMatch(pattern, input, WildcardPatternOptions.None, StringComparison.CurrentCulture);

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
    [Pure]
    public static bool IsMatch(string pattern, string input, WildcardPatternOptions options, StringComparison comparisonType)
    {
        var wildcard = new WildcardPattern(pattern, options, comparisonType);
        return wildcard.IsMatch(input);
    }

    /// <summary>Handles the actual matching.</summary>
    [Pure]
    private bool Match(Substring pattern, Substring input)
    {
        // Match if end of pattern or if we only have a '*' left.
        if (input.IsEnd())
        {
            return pattern.IsEnd() || (pattern.Ch == MultipleChars && pattern.Left == 1);
        }
        else if (pattern.IsEnd())
        {
            return false;
        }
        else return MatchChar(pattern, input);
    }

    [Pure]
    private bool MatchChar(Substring pattern, Substring input)
    {
        // If there is *, then there are two possibilities:
        // - We consider current character of second string.
        // - We ignore current character of second string.
        if (pattern.Ch == MultipleChars)
        {
            return Match(pattern.Next(), input) || Match(pattern, input.Next());
        }

        // If the first string contains '?'.
        else if (pattern.Ch == SingleChar)
        {
            return Options.HasFlag(WildcardPatternOptions.SingleOrTrailing)
                ? Match(pattern.Next(), input) || Match(pattern.Next(), input.Next())
                : Match(pattern.Next(), input.Next());
        }

        // If the current characters of both strings match.
        else return Equals(pattern.Ch, input.Ch) && Match(pattern.Next(), input.Next());
    }

    [Pure]
    private bool Equals(char l, char r)
    {
        if (l == r) return true;
        else if (IgnoreCase)
        {
            var culture = IsCultureIndependent ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;
            var ll = char.ToLower(l, culture);
            var rl = char.ToLower(r, culture);
            return ll == rl;
        }
        else return false;
    }

    /// <summary>Represents the wildcard pattern as debug string.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

    private readonly struct Substring(string str, int pos = 0)
    {
        public readonly int Position = pos;

        public readonly string Value = str;

        public char Ch => Value[Position];

        public int Left => Value.Length - Position;

        [Pure]
        public bool IsEnd() => Position >= Value.Length;

        [Pure]
        public Substring Next() => new(Value, Position + 1);
    }
}
