namespace System.Xml.Schema;

/// <summary>Extensions on <see cref="XmlSchemaSet"/>.</summary>
public static class QowaivXmlSchemaSetExtensions
{
    extension(XmlSchemaSet set)
    {
        /// <summary>Appends an <see cref="XmlSchema"/> to the <see cref="XmlSchemaSet"/>.</summary>
        [FluentSyntax]
        public XmlSchemaSet Append(XmlSchema schema)
        {
            Guard.NotNull(set).Add(Guard.NotNull(schema));
            return set;
        }
    }
}
