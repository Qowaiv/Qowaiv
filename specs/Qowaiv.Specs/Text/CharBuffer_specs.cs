namespace Text.CharBuffer_specs;

public class Unify
{
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
    public void removes_markup_diacritics_and_converts_to_uppercases(char ch)
    {
        ReadOnlySpan<char> span = $"{ch} Héllo,{ch}world!  ";
        Span<char> buffer = stackalloc char[span.BufferSize()];
        var length = span.Unify(buffer);
        var unified = buffer[..length].ToString();
        unified.Should().Be("HELLO,WORLD!");
    }
}
