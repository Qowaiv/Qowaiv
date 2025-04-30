partial struct @Svo : global::System.Xml.Serialization.IXmlSerializable
{
    /// <inheritdoc />
    [global::System.Diagnostics.Contracts.Pure]
    global::System.Xml.Schema.XmlSchema? global::System.Xml.Serialization.IXmlSerializable.GetSchema() => null;

    /// <inheritdoc />
    void global::System.Xml.Serialization.IXmlSerializable.ReadXml(global::System.Xml.XmlReader reader)
    {
#if NET6_0_OR_GREATER
            global::System.ArgumentNullException.ThrowIfNull(reader);
#else
        if (reader is null) throw new global::System.ArgumentNullException(nameof(reader));
#endif
        var xml = reader.ReadElementString();
        global::System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, global::System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    void global::System.Xml.Serialization.IXmlSerializable.WriteXml(global::System.Xml.XmlWriter writer)
    {
#if NET6_0_OR_GREATER
            global::System.ArgumentNullException.ThrowIfNull(writer);
#else
        if (writer is null) throw new global::System.ArgumentNullException(nameof(writer));
#endif
        writer.WriteString(behavior.ToXml(m_Value));
    }
}
