namespace Text.Wild_card_pattern_specs;

public class Is_match
{
    [Test]
    public void default_without_specials_for_current_culture()
        => WildcardPattern.IsMatch("%", "a").Should().BeFalse();

    [TestCase("*", "matches")]
    [TestCase("转*字", "转注字")]
    [TestCase("转?字", "转注字")]
    [TestCase("g*ks", "geeks")]
    [TestCase("g*éks", "geéks")]
    [TestCase("G*ks", "Geeks")]
    [TestCase("ge?ks*", "geeksforgeeks")]
    [TestCase("abc*bcd", "abcdhghgbcd")]
    [TestCase("*c*d", "abcd")]
    [TestCase("*?c*d", "abcd")]
    public void using_wildcards(string pattern, string input)
        => WildcardPattern.IsMatch(pattern, input)
        .Should().BeTrue();

    [TestCase("转%字", "转注字")]
    [TestCase("转_字", "转注字")]
    [TestCase("g%ks", "geeks")]
    [TestCase("g%éks", "geéks")]
    [TestCase("G%ks", "Geeks")]
    [TestCase("ge_ks%", "geeksforgeeks")]
    [TestCase("abc%bcd", "abcdhghgbcd")]
    [TestCase("%c%d", "abcd")]
    [TestCase("%_c%d", "abcd")]
    public void using_SQL_wildcards(string pattern, string input)
        => WildcardPattern.IsMatch(pattern, input, WildcardPatternOptions.SqlWildcards, StringComparison.Ordinal)
        .Should().BeTrue();

    [TestCase("gee?ks", "geeks")]
    [TestCase("ge?ks", "geeks")]
    [TestCase("Qowaiv?", "Qowaiv")]
    [TestCase("Qowaiv??", "Qowaiv")]
    [TestCase("Qowaiv*", "Qowaiv")]
    [TestCase("Qowaiv*?*", "Qowaiv")]
    public void with_trailing(string pattern, string input) 
        => WildcardPattern.IsMatch(pattern, input, WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal)
        .Should().BeTrue();

    [TestCase("gee_ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing | WildcardPatternOptions.SqlWildcards)]
    [TestCase("ge_ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing | WildcardPatternOptions.SqlWildcards)]
    [TestCase("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [TestCase("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [TestCase("Ig*ks", "iGeeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [TestCase("ig*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [TestCase("Ig*ks", "ıGeeks", false, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [TestCase("ıg*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [TestCase("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [TestCase("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [TestCase("Ig*ks", "iGeeks", false, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [TestCase("ig*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [TestCase("Ig*ks", "ıGeeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [TestCase("ıg*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [TestCase("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [TestCase("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [TestCase("Ig*ks", "iGeeks", false, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [TestCase("ig*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [TestCase("Ig*ks", "ıGeeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [TestCase("ıg*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    public void IsMatch(string pattern, string input, bool isMatch, WildcardPatternOptions options = default, StringComparison comparisonType = default, CultureInfo? culture = null)
    {
        using ((culture ?? CultureInfo.InvariantCulture).Scoped())
        {
            WildcardPattern.IsMatch(pattern, input, options, comparisonType)
                .Should().Be(isMatch);
        }
    }
}

public class Can_not_match
{
    [TestCase("gee?ks", "geeks", "No trailing")]
    [TestCase("g*ks", "Geeks", "Capitalization")]
    [TestCase("g*Ks", "geeks")]
    [TestCase("g*k", "gee", "'k' is not in second")]
    [TestCase("*pqrs", "pqrst", "'t' is not in first")]
    [TestCase("abc*c?d", "abcd", "must have 2 instances of 'c'")]
    [TestCase("*_c%d", "abcd", "SQL wildcards")]
    [TestCase("g%ks", "geeks", "SQL wildcards")]
    public void using_wildcards(string pattern, string input, string? because = null)
        => WildcardPattern.IsMatch(pattern, input)
        .Should().BeFalse(because);

    [TestCase("g%ks", "Geeks", "Capitalization")]
    [TestCase("g%Ks", "geeks")]
    [TestCase("g%k", "gee", "'k' is not in second")]
    [TestCase("%pqrs", "pqrst", "'t' is not in first")]
    [TestCase("abc*c?d", "abcd", "must have 2 instances of 'c'")]
    public void using_SQL_wildcards(string pattern, string input, string? because = null)
            => WildcardPattern.IsMatch(pattern, input, WildcardPatternOptions.SqlWildcards, StringComparison.Ordinal)
            .Should().BeFalse(because);
}

public class Can_not_have
{
    [Test]
    public void two_starts_in_a_row()
    {
        Func<WildcardPattern> create = () => new WildcardPattern("**");
        create.Should()
            .Throw<ArgumentException>()
            .WithMessage("The wildcard pattern is invalid.*");
    }
    [Test]
    public void two_percentage_signs_in_a_row_for_SQL()
    {
        Func<WildcardPattern> create = () => new WildcardPattern("%%", WildcardPatternOptions.SqlWildcards, StringComparison.CurrentCulture);
        create.Should()
            .Throw<ArgumentException>()
            .WithMessage("The wildcard pattern is invalid.*");
    }
}

public class Can_be_serialized
{
    public static readonly WildcardPattern TestPattern = new("t?st*", WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal);

#if NET8_0_OR_GREATER
#else
    [Test]
    public void ISerializer_interface()
    {
        ISerializable obj = TestPattern;
        var info = new SerializationInfo(typeof(Date), new FormatterConverter());
        obj.GetObjectData(info, default);

        info.GetString("Pattern").Should().Be("t?st*");
        info.GetInt32("Options").Should().Be((int)WildcardPatternOptions.SingleOrTrailing);
        info.GetInt32("ComparisonType").Should().Be((int)StringComparison.Ordinal);
    }

    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void BInary_serializer()
    {
        var act = SerializeDeserialize.Binary(TestPattern);
        act.Should().HaveDebuggerDisplay("{t?st*}, SingleOrTrailing, Ordinal");
    }
#endif

    [Test]
    public void DataContract_serializer()
    {
        var act = SerializeDeserialize.DataContract(TestPattern);
        act.Should().HaveDebuggerDisplay("{t?st*}, SingleOrTrailing, Ordinal");
    }
}
