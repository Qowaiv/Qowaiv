namespace Text.Wild_card_pattern_specs;

public class Is_match
{
    [Test]
    public void default_without_specials_for_current_culture()
        =>  WildcardPattern.IsMatch("%", "a").Should().BeFalse();
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
