using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Syntax;

namespace Code_Generation.Syntax.Literal_specs;

public class Can_represent
{
    [Test]
    public void _Nill()
        => new Literal(Nill.Value).ToString().Should().Be("null");

    [Test]
    public void _null()
        => new Literal(null).ToString().Should().Be("null");

    [Test]
    [Arguments(typeof(int), "typeof(int)")]
    [Arguments(typeof(Literal), "typeof(Qowaiv.CodeGeneration.Syntax.Literal)")]
    public void Types(Type type, string code)
        => new Literal(type).ToString().Should().Be(code);

    [Test]
    public void boolean()
        => new Literal(true).ToString().Should().Be("true");

    [Test]
    public void _int()
        => new Literal(42).ToString().Should().Be("42");

    [Test]
    public void _double()
        => new Literal(3.1415926535897).ToString().Should().Be("3.1415926535897");

    [Test]
    public void _decimal()
        => new Literal(3.1415926535897931m).ToString().Should().Be("3.1415926535897931m");

    [Test]
    public void _string()
        => new Literal("Hello, World!").ToString().Should().Be("\"Hello, World!\"");

    [Test]
    public void _enum()
        => new Literal(TypeCode.Int16).ToString().Should().Be("System.TypeCode.Int16");
}

public class Throws
{
    [Test]
    public void on_not_supported()
        => new object().Invoking(o => new Literal(o).ToString())
        .Should().Throw<NotSupportedException>();
}
