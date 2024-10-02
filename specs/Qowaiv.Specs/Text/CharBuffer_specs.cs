namespace Text.CharBuffer_specs;

public class Add
{
    private static CharBuffer WithCapacity() => CharBuffer.Empty(16);

    [Test]
    public void A_char_can_be_added()
    {
        var buffer = WithCapacity().Add('c');
        buffer.Should().BeEquivalentTo("c");
    }

    [Test]
    public void A_char_can_be_added_as_lowercase()
    {
        var buffer = WithCapacity().AddLower('C');
        buffer.Should().BeEquivalentTo("c");
    }

    [Test]
    public void A_string_can_be_added()
    {
        var buffer = WithCapacity().Add("string");
        buffer.Should().BeEquivalentTo("string");
    }

    [Test]
    public void A_CharBuffer_can_be_added()
    {
        var other = " string ".Buffer().Trim();
        var buffer = WithCapacity().Add(other);
        buffer.Should().BeEquivalentTo("string");
    }
}

public class Remove
{
    [Test]
    public void FromStart_removes_characters_from_the_start()
    {
        var buffer = " test ".Buffer().Trim().RemoveFromStart(2);
        buffer.Should().BeEquivalentTo("st");
    }

    [Test]
    public void FromEnd_removes_characters_from_the_end()
    {
        var buffer = " test ".Buffer().Trim().RemoveFromEnd(2);
        buffer.Should().BeEquivalentTo("te");
    }

    [Test]
    public void Range_removes_specified_range()
    {
        var buffer = " test ".Buffer().Trim().RemoveRange(1, 2);
        buffer.Should().BeEquivalentTo("tt");
    }

    [TestCase('.')]
    [TestCase('-')]
    [TestCase('_')]
    [TestCase((char)0x00B7)] // middle dot
    [TestCase((char)0x22C5)] // dot operator
    [TestCase((char)0x2202)] // bullet
    [TestCase((char)0x2012)] // figure dash / minus
    [TestCase((char)0x2013)] // en dash
    [TestCase((char)0x2014)] // em dash
    [TestCase((char)0x2015)] // horizontal bar
    public void Markup_removes_all_markup_chars(char ch)
    {
        var buffer = $"{ch} Hello,{ch}world!  ".Buffer().RemoveMarkup();
        buffer.Should().BeEquivalentTo("Hello,world!");
    }
}

public class Clear
{
    [Test]
    public void Empties_the_buffer()
    {
        "test".Buffer().Clear().IsEmpty().Should().BeTrue();
    }
}

public class Trim
{
    [Test]
    public void Trims_left_and_right_spaces()
    {
        var buffer = "  content ".Buffer().Trim();
        buffer.Should().BeEquivalentTo("content");
    }

    [Test]
    public void Left_trims_left_spaces()
    {
        var buffer = "  content ".Buffer().TrimLeft();
        buffer.Should().BeEquivalentTo("content ");
    }

    [Test]
    public void Right_trims_right_spaces()
    {
        var buffer = "  content ".Buffer().TrimRight();
        buffer.Should().BeEquivalentTo("  content");
    }
}

public class Uppercase
{
    [Test]
    public void Transforms_all_characters_to__uppercase_alternatives()
    {
        var buffer = "abcDeéf".Buffer().Uppercase();
        buffer.Should().BeEquivalentTo("ABCDEÉF");
    }
}

public class Count
{
    [Test]
    public void Returns_the_total_of_specified_char()
    {
        var buffer = " let us count this   ".Buffer().Trim();
        buffer.Count(' ').Should().Be(3);
    }
}

public class IndexOf
{
    [Test]
    public void Last_returns_last_index_of_char()
    {
        var buffer = " 7123456789   ".Buffer().Trim();
        buffer.LastIndexOf('7').Should().Be(7);
    }
}

public class StartsWith_String
{
    [Test]
    public void Longer_then_buffer_is_false()
    {
        var buffer = " test".Buffer().RemoveFromEnd(1);
        buffer.StartsWithCaseInsensitive("testx").Should().BeFalse();
    }

    [Test]
    public void That_matches_buffer_but_not_al_visible_is_false()
    {
        var buffer = " test".Buffer().Trim().RemoveFromEnd(1);
        buffer.StartsWithCaseInsensitive("test").Should().BeFalse();
    }

    [Test]
    public void Not_matching_buffer_is_false()
    {
        var buffer = " test".Buffer().Trim();
        buffer.StartsWithCaseInsensitive("xe").Should().BeFalse();
    }

    [Test]
    public void Matching_buffer_is_true()
    {
        var buffer = " test".Buffer().Trim();
        buffer.StartsWithCaseInsensitive("tes").Should().BeTrue();
    }
}

public class Substring
{
    [Test]
    public void With_start_index_returns_substring()
    {
        var buffer = " test ".Buffer().Trim();
        buffer.Substring(2).Should().Be("st");
    }
}

public class IsEmpty
{
    [Test]
    public void Is_true_for_buffer_without_visible_content()
    {
        var buffer = "   ".Buffer().Trim();
        buffer.IsEmpty().Should().BeTrue();
    }
}
