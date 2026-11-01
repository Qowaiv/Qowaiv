using AwesomeAssertions;

namespace Text.Chars_specs;

public class Is
{
    [TestCase('0')]
    [TestCase('1')]
    [TestCase('8')]
    [TestCase('9')]
    public void _0_9(char c) => Chars.get_Is_0_9(c).Should().BeTrue();

    [TestCase('a')]
    [TestCase('b')]
    [TestCase('y')]
    [TestCase('z')]
    public void _a_z(char c) => Chars.get_Is_a_z(c).Should().BeTrue();

    [TestCase('A')]
    [TestCase('B')]
    [TestCase('Y')]
    [TestCase('Z')]
    public void _A_Z(char c) => Chars.get_Is_A_Z(c).Should().BeTrue();
}

public class Is_Not
{
    [TestCase((char)('0' - 1))]
    [TestCase((char)('9' + 1))]
    [TestCase('A')]
    [TestCase('a')]
    public void _0_9(char c) => Chars.get_Is_0_9(c).Should().BeFalse();

    [TestCase((char)('a' - 1))]
    [TestCase((char)('z' + 1))]
    [TestCase('A')]
    [TestCase('9')]
    [TestCase('!')]
    public void _a_z(char c) => Chars.get_Is_a_z(c).Should().BeFalse();

    [TestCase((char)('A' - 1))]
    [TestCase((char)('Z' + 1))]
    [TestCase('a')]
    [TestCase('9')]
    [TestCase('!')]
    public void _A_Z(char c) => Chars.get_Is_A_Z(c).Should().BeFalse();
}
