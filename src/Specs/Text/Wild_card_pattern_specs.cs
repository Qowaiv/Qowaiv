namespace Text.Wild_card_pattern_specs;

public class Is_match
{
    [Test]
    public void default_without_specials_for_current_culture()
        => WildcardPattern.IsMatch("%", "a").Should().BeFalse();

    [Test]
    [Arguments("*", "matches")]
    [Arguments("转*字", "转注字")]
    [Arguments("转?字", "转注字")]
    [Arguments("g*ks", "geeks")]
    [Arguments("g*éks", "geéks")]
    [Arguments("G*ks", "Geeks")]
    [Arguments("ge?ks*", "geeksforgeeks")]
    [Arguments("abc*bcd", "abcdhghgbcd")]
    [Arguments("*c*d", "abcd")]
    [Arguments("*?c*d", "abcd")]
    public void using_wildcards(string pattern, string input)
        => WildcardPattern.IsMatch(pattern, input)
        .Should().BeTrue();

    [Test]
    [Arguments("转%字", "转注字")]
    [Arguments("转_字", "转注字")]
    [Arguments("g%ks", "geeks")]
    [Arguments("g%éks", "geéks")]
    [Arguments("G%ks", "Geeks")]
    [Arguments("ge_ks%", "geeksforgeeks")]
    [Arguments("abc%bcd", "abcdhghgbcd")]
    [Arguments("%c%d", "abcd")]
    [Arguments("%_c%d", "abcd")]
    public void using_SQL_wildcards(string pattern, string input)
        => WildcardPattern.IsMatch(pattern, input, WildcardPatternOptions.SqlWildcards, StringComparison.Ordinal)
        .Should().BeTrue();

    [Test]
    [Arguments("gee?ks", "geeks")]
    [Arguments("ge?ks", "geeks")]
    [Arguments("Qowaiv?", "Qowaiv")]
    [Arguments("Qowaiv??", "Qowaiv")]
    [Arguments("Qowaiv*", "Qowaiv")]
    [Arguments("Qowaiv*?*", "Qowaiv")]
    public void with_trailing(string pattern, string input)
        => WildcardPattern.IsMatch(pattern, input, WildcardPatternOptions.SingleOrTrailing, StringComparison.Ordinal)
        .Should().BeTrue();

    [Test]
    [Arguments("gee_ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing | WildcardPatternOptions.SqlWildcards)]
    [Arguments("ge_ks", "geeks", true, WildcardPatternOptions.SingleOrTrailing | WildcardPatternOptions.SqlWildcards)]
    [Arguments("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [Arguments("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [Arguments("Ig*ks", "iGeeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [Arguments("ig*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [Arguments("Ig*ks", "ıGeeks", false, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [Arguments("ıg*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.InvariantCultureIgnoreCase, "tr-TR")]
    [Arguments("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [Arguments("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [Arguments("Ig*ks", "iGeeks", false, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [Arguments("ig*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [Arguments("Ig*ks", "ıGeeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [Arguments("ıg*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.CurrentCultureIgnoreCase, "tr-TR")]
    [Arguments("g*ks", "Geeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [Arguments("g*Ks", "geeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [Arguments("Ig*ks", "iGeeks", false, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [Arguments("ig*ks", "IGeeks", false, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [Arguments("Ig*ks", "ıGeeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
    [Arguments("ıg*ks", "IGeeks", true, WildcardPatternOptions.None, StringComparison.OrdinalIgnoreCase, "tr-TR")]
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
    [Test]
    [Arguments("gee?ks", "geeks", "No trailing")]
    [Arguments("g*ks", "Geeks", "Capitalization")]
    [Arguments("g*Ks", "geeks")]
    [Arguments("g*k", "gee", "'k' is not in second")]
    [Arguments("*pqrs", "pqrst", "'t' is not in first")]
    [Arguments("abc*c?d", "abcd", "must have 2 instances of 'c'")]
    [Arguments("*_c%d", "abcd", "SQL wildcards")]
    [Arguments("g%ks", "geeks", "SQL wildcards")]
    public void using_wildcards(string pattern, string input, string? because = null)
        => WildcardPattern.IsMatch(pattern, input)
        .Should().BeFalse(because);

    [Test]
    [Arguments("g%ks", "Geeks", "Capitalization")]
    [Arguments("g%Ks", "geeks")]
    [Arguments("g%k", "gee", "'k' is not in second")]
    [Arguments("%pqrs", "pqrst", "'t' is not in first")]
    [Arguments("abc*c?d", "abcd", "must have 2 instances of 'c'")]
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
