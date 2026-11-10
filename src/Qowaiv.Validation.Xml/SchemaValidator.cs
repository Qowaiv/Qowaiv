using System.IO;

namespace Qowaiv.Validation.Xml;

/// <summary>Implements <see cref="IValidator{TModel}"/> using <see cref="XmlSchema"/>'s.</summary>
/// <typeparam name="TModel">
/// The type of the model to validate.
/// </typeparam>
/// <remarks>Initializes a new instance of the <see cref="SchemaValidator{TModel}"/> class.</remarks>
/// <param name="schemas">
/// A set of <see cref="XmlSchema"/>'s.
/// </param>
public sealed class SchemaValidator<TModel>(XmlSchemaSet schemas) : IValidator<TModel>
    where TModel : class
{
    /// <summary>Initializes a new instance of the <see cref="SchemaValidator{TModel}"/> class.</summary>
    /// <param name="stream">
    /// A stream representing an <see cref="XmlSchema"/>.
    /// </param>
    public SchemaValidator(Stream stream) : this(stream.AsXmlSchema()) { }

    /// <summary>Initializes a new instance of the <see cref="SchemaValidator{TModel}"/> class.</summary>
    /// <param name="schema">
    /// The <see cref="XmlSchema"/> to validate against.
    /// </param>
    public SchemaValidator(XmlSchema schema) : this(new XmlSchemaSet().Append(schema)) { }

    /// <summary>The XML Schema('s) used to validate the model.</summary>
    public XmlSchemaSet Schemas { get; } = Guard.NotNull(schemas);

    /// <summary>Validates the model using the <see cref="XmlSchemaSet"/>.</summary>
    [Pure]
    public Result<TModel> Validate(TModel model)
    {
        using var stream = new MemoryStream();
        var writer = XmlWriter.Create(stream);
        Serializer.Serialize(writer, model);
        stream.Position = 0;
        var messages = XDocument.Load(stream).Validate(Schemas).Messages;
        return Result.For(model, messages);
    }

    /// <summary>XML deserialize the model using the <see cref="XmlSchemaSet"/> to validate.</summary>
    [Pure]
    public Result<TModel> Deserialize(Stream stream)
        => XDocument.Load(stream)
        .Validate(Schemas)
        .Act(Deserialize);

    /// <summary>XML deserialize the model using the <see cref="XmlSchemaSet"/> to validate.</summary>
    [Pure]
    public Result<TModel> Deserialize(string xml)
        => XDocument.Parse(xml)
        .Validate(Schemas)
        .Act(Deserialize);

    [Pure]
    private Result<TModel> Deserialize(XDocument document)
        => (TModel)Serializer.Deserialize(document.CreateReader())!;

    private readonly XmlSerializer Serializer = new(typeof(TModel));
}
