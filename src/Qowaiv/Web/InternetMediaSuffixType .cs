namespace Qowaiv.Web;

/// <summary>Internet media type suffixes.</summary>
/// <remarks>
/// See: https://tools.ietf.org/html/rfc6839
/// See: https://tools.ietf.org/html/rfc7049.
/// </remarks>
public enum InternetMediaSuffixType
{
    /// <summary>None (default).</summary>
    None = 0,
    /// <summary>XML (Extensible Markup Language).</summary>
    xml,
    /// <summary>JSON (JavaScript Object Notation).</summary>
    json,
    /// <summary>BER (Binary Encoding).</summary>
    ber,
    /// <summary>DER (Distinguished Encoding Rules).</summary>
    der,
    /// <summary>Fast Infoset document format (binary representation of the XML Information Set).</summary>
    fastinfoset,
    /// <summary>WBML (Wireless Application Protocol Binary XML).</summary>
    wbxml,
    /// <summary>ZIP (compression).</summary>
    zip,
    /// <summary>CBOR (Concise Binary Object Representation).</summary>
    cbor,
}
