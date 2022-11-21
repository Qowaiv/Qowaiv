namespace Qowaiv;

/// <summary>Represents a collection of unique email addresses.</summary>
/// <remarks>
/// Empty and unknown email addresses can not be added.
/// </remarks>
[Serializable]
[OpenApiDataType(description: "Comma separated list of email addresses defined by RFC 5322.", example: "info@qowaiv.org,test@test.com", type: "string", format: "email-collection", nullable: true)]
[OpenApi.OpenApiDataType(description: "Comma separated list of email addresses defined by RFC 5322.", example: "info@qowaiv.org,test@test.com", type: "string", format: "email-collection", nullable: true)]
public class EmailAddressCollection : ISet<EmailAddress>, ISerializable, IXmlSerializable, IFormattable
#if NET7_0_OR_GREATER
    , IParsable<EmailAddressCollection>
#endif
{
    /// <summary>The email address separator is a comma.</summary>
    /// <remarks>
    /// According to RFC 6068: http://tools.ietf.org/html/rfc6068.
    /// </remarks>
    public const string Separator = ",";
    /// <summary>The separators used for parsing.</summary>
    /// <remarks>
    /// Both comma and semicolon are supported.
    /// </remarks>
    private static readonly char[] Separators = new[] { ',', ';' };

    /// <summary>The underlying hash set.</summary>
    /// <remarks>
    /// The only proper way to block the adding of empty and unknown 
    /// email addresses was by overriding Add, which is not allowed if
    /// derived from HasSet&lt;EmailAddress&gt;.
    /// 
    /// So this construction is required.
    /// </remarks>
    private readonly HashSet<EmailAddress> hashset = new();

    /// <summary>Initiates a new collection of email addresses.</summary>
    public EmailAddressCollection() { }

    /// <summary>Initiates a new collection of email addresses.</summary>
    /// <param name="emails">
    /// An array of email addresses.
    /// </param>
    public EmailAddressCollection(params EmailAddress[] emails)
        : this((IEnumerable<EmailAddress>)emails) { }

    /// <summary>Initiates a new collection of email addresses.</summary>
    /// <param name="emails">
    /// An enumeration of email addresses.
    /// </param>
    public EmailAddressCollection(IEnumerable<EmailAddress> emails)
        : this() => AddRange(emails);

    #region Methods

    /// <summary>Adds an email address to the current collection and returns
    /// a value to indicate if the email address was successfully added.
    /// </summary>
    /// <param name="item">
    /// The email address to add.
    /// </param>
    [CollectionMutation]
    public bool Add(EmailAddress item)
    {
        if (item.IsEmptyOrUnknown()) { return false; }
        return hashset.Add(item);
    }

    /// <summary>Keeps only the distinct set of email addresses in the collection.</summary>
    /// <returns>
    /// The current (cleaned) instance of the collection.
    /// </returns>
    public void AddRange(IEnumerable<EmailAddress> emails)
    {
        Guard.NotNull(emails, nameof(emails));
        foreach (var email in emails) { Add(email); }
    }

    #endregion

    #region ICollection

    /// <summary>Gets the number of email addresses in the collection.</summary>
    [ExcludeFromCodeCoverage]
    public int Count => hashset.Count;

    /// <summary>Returns false as this collection is not read only.</summary>
    [ExcludeFromCodeCoverage]
    public bool IsReadOnly => false;

    /// <summary>Adds an email address to the current collection.</summary>
    [ExcludeFromCodeCoverage]

    void ICollection<EmailAddress>.Add(EmailAddress email) => Add(email);

    /// <summary>Clears all email addresses From current collection.</summary>
    [ExcludeFromCodeCoverage]
    public void Clear() => hashset.Clear();

    /// <summary>Returns true if the collection contains the specified email address.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool Contains(EmailAddress item) => hashset.Contains(item);

    /// <summary>Copies the email addresses of the collection to an
    /// System.Array, starting at a particular System.Array index.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void CopyTo(EmailAddress[] array, int arrayIndex) => hashset.CopyTo(array, arrayIndex);

    /// <summary>Removes the email address from the collection.</summary>
    [ExcludeFromCodeCoverage]
    [CollectionMutation]
    public bool Remove(EmailAddress item) => hashset.Remove(item);

    /// <summary>Gets an enumerator to loop through all email addresses of the collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public IEnumerator<EmailAddress> GetEnumerator() => hashset.GetEnumerator();

    /// <summary>Gets an enumerator to loop through all email addresses of the collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region ISet

    /// <summary>Removes all elements in the specified collection from the current set.</summary>
    [ExcludeFromCodeCoverage]
    public void ExceptWith(IEnumerable<EmailAddress> other) => hashset.ExceptWith(other);

    /// <summary>Modifies the current set so that it contains only elements that are also in a specified collection.</summary>
    [ExcludeFromCodeCoverage]
    public void IntersectWith(IEnumerable<EmailAddress> other) => hashset.IntersectWith(other);

    /// <summary>Determines whether the current set is a proper (strict) subset of a specified collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool IsProperSubsetOf(IEnumerable<EmailAddress> other) => hashset.IsProperSubsetOf(other);

    /// <summary>Determines whether the current set is a proper (strict) superset of a specified collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool IsProperSupersetOf(IEnumerable<EmailAddress> other) => hashset.IsProperSupersetOf(other);

    /// <summary>Determines whether a set is a subset of a specified collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool IsSubsetOf(IEnumerable<EmailAddress> other) => hashset.IsSubsetOf(other);

    /// <summary>Determines whether a set is a superset of a specified collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool IsSupersetOf(IEnumerable<EmailAddress> other) => hashset.IsSupersetOf(other);

    /// <summary>Determines whether the current set overlaps with the specified collection.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool Overlaps(IEnumerable<EmailAddress> other) => hashset.Overlaps(other);

    /// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
    [ExcludeFromCodeCoverage]
    [Pure]
    public bool SetEquals(IEnumerable<EmailAddress> other) => hashset.SetEquals(other);

    /// <summary>Modifies the current set so that it contains only elements that are present
    /// either in the current set or in the specified collection, but not both.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void SymmetricExceptWith(IEnumerable<EmailAddress> other) => hashset.SymmetricExceptWith(other);

    /// <summary>Modifies the current set so that it contains all elements that are present
    /// in either the current set or the specified collection.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void UnionWith(IEnumerable<EmailAddress> other) => hashset.UnionWith(other);

    #endregion

    #region Serialization

    /// <summary>Initializes a new instance of email address based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    protected EmailAddressCollection(SerializationInfo info, StreamingContext context)
        : this()
    {
        Guard.NotNull(info, nameof(info));
        AddRange(Parse(info.GetString("Value"), CultureInfo.InvariantCulture));
    }

    /// <summary>Adds the underlying property of email address to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) { GetObjectData(info, context); }

    /// <summary>Adds the underlying property of email address to the serialization info.</summary>
    /// <remarks>
    /// this is used by ISerializable.GetObjectData() so that it can be changed by derived classes.
    /// </remarks>
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        info.AddValue("Value", ToString());
    }

    /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize an email address.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => GetSchema();

    /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize an email address.</summary>
    /// <remarks>
    /// this is used by IXmlSerializable.GetSchema() so that it can be changed by derived classes.
    /// </remarks>
    [Pure]
    protected virtual XmlSchema? GetSchema() => null;

    /// <summary>Reads the email address from an <see href="XmlReader"/>.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader) { ReadXml(reader); }

    /// <summary>Reads the email address from an <see href="XmlReader"/>.</summary>
    /// <param name="reader">An XML reader.</param>
    /// <remarks>
    /// this is used by IXmlSerializable.ReadXml() so that it can be changed by derived classes.
    /// </remarks>
    protected virtual void ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader, nameof(reader));
        var s = reader.ReadElementString();
        var val = Parse(s, CultureInfo.InvariantCulture);
        AddRange(val);
    }

    /// <summary>Writes the email address to an <see href="XmlWriter"/>.</summary>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer) { WriteXml(writer); }
    /// <summary>Writes the email address to an <see href="XmlWriter"/>.</summary>
    /// <remarks>
    /// this is used by IXmlSerializable.WriteXml() so that it can be
    /// changed by derived classes.
    /// </remarks>
    protected virtual void WriteXml(XmlWriter writer)
    {
        Guard.NotNull(writer, nameof(writer));
        writer.WriteString(ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Deserializes the email address collection from a JSON string.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized email address collection.
    /// </returns>
    [Pure]
    public static EmailAddressCollection FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);

    /// <summary>Serializes the email address collection to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public virtual string? ToJson() => Count == 0 ? null : ToString(CultureInfo.InvariantCulture);

    #endregion

    #region IFormattable / ToString

    /// <summary>Returns a <see cref="string"/> that represents the current email address collection.</summary>
    [Pure]
    public override string ToString() => ToString(CultureInfo.CurrentCulture);

    /// <summary>Returns a formatted <see cref="string"/> that represents the current email address collection.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

    /// <summary>Returns a formatted <see cref="string"/> that represents the current email address collection.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider provider) => ToString("", provider);

    /// <summary>Returns a formatted <see cref="string"/> that represents the current email address collection.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? provider)
        => StringFormatter.TryApplyCustomFormatter(format, this, provider, out string formatted)
        ? formatted
        : string.Join(Separator, this.Select(emailaddress => emailaddress.ToString(format, provider)));

    #endregion

    #region Factory methods

    /// <summary>Converts the string to an email address collection.</summary>
    /// <param name="s">
    /// A string containing an email address to convert.
    /// </param>
    /// <returns>
    /// An email address.
    /// </returns>
    /// <exception cref="FormatException">
    /// s is not in the correct format.
    /// </exception>
    [Pure]
    public static EmailAddressCollection Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

    /// <summary>Converts the string to an email address collection.</summary>
    /// <param name="s">
    /// A string containing an email address to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// An email address.
    /// </returns>
    /// <exception cref="FormatException">
    /// s is not in the correct format.
    /// </exception>
    [Pure]
    public static EmailAddressCollection Parse(string? s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out EmailAddressCollection val))
        {
            return val;
        }
        throw new FormatException(QowaivMessages.FormatExceptionEmailAddressCollection);
    }

    /// <summary>Converts the string to an email address collection.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an email address to convert.
    /// </param>
    /// <returns>
    /// The email address if the string was converted successfully, otherwise an empty EmailAddressCollection().
    /// </returns>
    [Pure]
    public static EmailAddressCollection TryParse(string s)
    {
        if (TryParse(s, out EmailAddressCollection val))
        {
            return val;
        }
        return new EmailAddressCollection();
    }

    /// <summary>Converts the string to an email address collection.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an email address to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string s, out EmailAddressCollection result)
    {
        return TryParse(s, CultureInfo.CurrentCulture, out result);
    }

    /// <summary>Converts the string to an email address collection.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an email address to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out EmailAddressCollection result)
    {
        result = new EmailAddressCollection();
        if (s is { Length: > 0 })
        {
            var strs = s.Split(Separators, StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim());
            foreach (var str in strs)
            {
                if (EmailAddress.TryParse(str, provider, out EmailAddress email))
                {
                    result.Add(email);
                }
                else
                {
                    result.Clear();
                    return false;
                }
            }
            return result.Any();
        }
        else return true;
    }

    #endregion
}
