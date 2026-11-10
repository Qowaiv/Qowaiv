using Qowaiv.Validation.Xml;
using Specs.Xml;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XML_schema_hardening_specs;

public class Prohibits_DTD
{
    private const string SchemaWithInternalEntity = """
        <?xml version="1.0"?>
        <!DOCTYPE xs:schema [
          <!ENTITY xxe "injected">
        ]>
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="&xxe;" type="xs:string"/>
        </xs:schema>
        """;

    private const string SchemaWithExternalEntity = """
        <?xml version="1.0"?>
        <!DOCTYPE xs:schema [
          <!ENTITY xxe SYSTEM "file:///etc/passwd">
        ]>
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="&xxe;" type="xs:string"/>
        </xs:schema>
        """;

    private static MemoryStream Schema(string content) => new(Encoding.UTF8.GetBytes(content));

    [Test]
    public void when_constructing_a_SchemaValidator_with_an_internal_entity()
    {
        Func<object> create = () => new SchemaValidator<Bookstore>(Schema(SchemaWithInternalEntity));
        create.Should().Throw<XmlException>().WithMessage("*DTD is prohibited*");
    }

    [Test]
    public void when_constructing_a_SchemaValidator_with_an_external_entity()
    {
        Func<object> create = () => new SchemaValidator<Bookstore>(Schema(SchemaWithExternalEntity));
        create.Should().Throw<XmlException>().WithMessage("*DTD is prohibited*");
    }

    [Test]
    public void when_validating_an_XDocument()
    {
        var document = XDocument.Parse("<bookstore/>");
        Func<object> validate = () => document.Validate(Schema(SchemaWithInternalEntity));
        validate.Should().Throw<XmlException>().WithMessage("*DTD is prohibited*");
    }

    [Test]
    public void accepts_a_schema_without_a_DTD()
        => new SchemaValidator<Bookstore>(Embedded.Stream("Specs/Xml/Files/schema.xsd"))
        .Should().NotBeNull();
}
