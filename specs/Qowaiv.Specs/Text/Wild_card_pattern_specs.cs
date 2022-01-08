namespace Text.Wild_card_pattern_specs;

public class Is_match
{
    [Test]
    public void default_without_specials_for_current_culture()
        =>  WildcardPattern.IsMatch("%", "a").Should().BeFalse();
}
