namespace System.Xml.Schema;

/// <summary>Extensions on <see cref="XmlSeverityType"/>.</summary>
public static class QowaivSeverityExtensions
{
    extension(XmlSeverityType severity)
    {
        /// <summary>Converts <see cref="XmlSeverityType"/> to <see cref="ValidationSeverity"/>.</summary>
        [Pure]
        public ValidationSeverity ValidationSeverity
            => severity is XmlSeverityType.Warning
            ? ValidationSeverity.Warning
            : ValidationSeverity.Error;
    }
}
