namespace System.IO;

internal static class QowaivValidationStreamExtensions
{
    extension(Stream stream)
    {
        [Pure]
        public XmlSchema AsXmlSchema()
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
            };
            using var reader = XmlReader.Create(stream, settings);
            return XmlSchema.Read(reader, null)!;
        }
    }
}
