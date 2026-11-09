using Qowaiv.Financial;
using System.Xml.Serialization;

namespace Specs.Xml;

[XmlRoot(ElementName = "bookstore", Namespace = "http://www.contoso.com/books")]
public class Bookstore
{
    [XmlElement("book")]
    public List<Book> Books { get; set; } = [];
}
public class Book
{
    [XmlElement("title")]
    public string? Title { get; set; }

    [XmlElement("author")]
    public Author? Author { get; set; }

    [XmlElement("price")]
    public Amount Price { get; set; }

    [XmlAttribute("genre")]
    public string? Genre { get; set; }

    [XmlAttribute("publicationdate")]
    public string? PublicationDate { get; set; }

    [XmlAttribute("ISBN")]
    public string? ISBN { get; set; }
}
public class Author
{
    [XmlElement("name")]
    public string? Name { get; set; }

    [XmlElement("first-name")]
    public string? FirstName { get; set; }

    [XmlElement("last-name")]
    public string? LastName { get; set; }
}
