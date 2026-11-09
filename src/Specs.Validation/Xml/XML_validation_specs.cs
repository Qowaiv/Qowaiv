using Qowaiv.Financial;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Xml;
using Specs.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XML_validation_specs;

public class Invalidates
{
    private static Stream Schema => Embedded.Stream("Specs/Xml/Files/schema.xsd");
    private static Stream Xml => Embedded.Stream("Specs/Xml/Files/invalid-document.xml");
    private static Stream XmlWithNamespacePrefix => Embedded.Stream("Specs/Xml/Files/invalid-document-namespace.xml");

    [Test]
    public void model()
    {
        var model = new Bookstore
        {
            Books = [new Book { Genre = "fiction", Price = 11.99m.Amount(), PublicationDate = "not-a-date" }]
        };
        var validator = new SchemaValidator<Bookstore>(Schema);
        validator.Validate(model)
            .Should().BeInvalid()
            .WithMessages(
                ValidationMessage.Error("The 'publicationdate' attribute is invalid - The value 'not-a-date' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:date' - The string 'not-a-date' is not a valid Date value.", "/bookstore/book[1]/@publicationdate"),
                ValidationMessage.Error("The required attribute 'ISBN' is missing.", "/bookstore/book[1]"),
                ValidationMessage.Error("The element 'book' in namespace 'http://www.contoso.com/books' has invalid child element 'price' in namespace 'http://www.contoso.com/books'. List of possible elements expected: 'title' in namespace 'http://www.contoso.com/books'.", "/bookstore/book[1]/price[1]"));
    }

    [Test]
    public void deserialized_model()
        => new SchemaValidator<Bookstore>(Schema)
        .Deserialize(Xml)
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("The required attribute 'genre' is missing.", "/bookstore/book[4]"),
            ValidationMessage.Error("The required attribute 'publicationdate' is missing.", "/bookstore/book[4]"),
            ValidationMessage.Error("The required attribute 'ISBN' is missing.", "/bookstore/book[4]"),
            ValidationMessage.Error("The element 'book' in namespace 'http://www.contoso.com/books' has invalid child element 'price' in namespace 'http://www.contoso.com/books'. List of possible elements expected: 'author' in namespace 'http://www.contoso.com/books'.", "/bookstore/book[4]/price[1]"));

    [Test]
    public void an_XDocument()
        => XDocument.Load(Xml)
        .Validate(Schema)
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("The required attribute 'genre' is missing.", "/bookstore/book[4]"),
            ValidationMessage.Error("The required attribute 'publicationdate' is missing.", "/bookstore/book[4]"),
            ValidationMessage.Error("The required attribute 'ISBN' is missing.", "/bookstore/book[4]"),
            ValidationMessage.Error("The element 'book' in namespace 'http://www.contoso.com/books' has invalid child element 'price' in namespace 'http://www.contoso.com/books'. List of possible elements expected: 'author' in namespace 'http://www.contoso.com/books'.", "/bookstore/book[4]/price[1]"));

    [Test]
    public void communicating_namespace_prefix_if_any()
        => XDocument.Load(XmlWithNamespacePrefix)
        .Validate(Schema)
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("The required attribute 'genre' is missing.", "/bk:bookstore/bk:book[2]"),
            ValidationMessage.Error("The required attribute 'publicationdate' is missing.", "/bk:bookstore/bk:book[2]"),
            ValidationMessage.Error("The required attribute 'ISBN' is missing.", "/bk:bookstore/bk:book[2]"),
            ValidationMessage.Error("The element 'book' in namespace 'http://www.contoso.com/books' has invalid child element 'price' in namespace 'http://www.contoso.com/books'. List of possible elements expected: 'author' in namespace 'http://www.contoso.com/books'.", "/bk:bookstore/bk:book[2]/bk:price[1]"));
}

public class Validates
{
    private static Stream Schema => Embedded.Stream("Specs/Xml/Files/schema.xsd");
    private static Stream Xml => Embedded.Stream("Specs/Xml/Files/valid-document.xml");

    [Test]
    public void model()
    {
        var model = new Bookstore
        {
            Books =
            [
                new Book 
                {
                    Genre = "fiction",
                    Author = new()
                    {
                        Name = "Douglas Adams",
                        LastName = "Adams",
                        FirstName = "Douglas",
                    },
                    Title = "The Hitchhiker's Guide to the Galaxy",
                    Price = 11.99m.Amount(), 
                    ISBN = "0-330-25864-8",
                    PublicationDate = "1979-10-12" 
                },
            ]
        };
        var validator = new SchemaValidator<Bookstore>(Schema);
        validator.Validate(model)
            .Should().BeValid()
            .WithoutMessages();
    }

    [Test]
    public void deserialized_model()
        => new SchemaValidator<Bookstore>(Schema)
        .Deserialize(Xml)
        .Should().BeValid().WithoutMessages();

    [Test]
    public void an_XDocument()
        => XDocument.Load(Xml)
        .Validate(Schema)
        .Should().BeValid().WithoutMessages();
}

public class Invalid_XML
{
    private static Stream Schema => Embedded.Stream("Specs/Xml/Files/schema.xsd");

    [Test]
    public void throws_via_XDocument()
    {
        var validator = new SchemaValidator<Bookstore>(Schema);
        Func<object> deserialize = () => validator.Deserialize("<invalid xml");
        deserialize.Should().Throw<System.Xml.XmlException>();
    }
}
