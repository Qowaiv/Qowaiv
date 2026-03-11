public partial struct @TSvo : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the @FullName.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the @FullName from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
#if !NotCultureDependent // exec
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
#else // exec
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml);
#endif // exec
    }

    /// <summary>Writes the @FullName to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}
