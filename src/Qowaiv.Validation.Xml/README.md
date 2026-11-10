# Qowaiv Validation Xml

XML/XSD schema validation integrated with the `Result<T>` abstraction.
Supports validating `XDocument` instances directly and round-trip validation
of models via `SchemaValidator<TModel>`.

## Validating an XDocument

Validate an XML document against one or more XSD schemas:

``` C#
var document = XDocument.Parse("<some xml />");
Result<XDocument> result = document.Validate(schema);
```

The schema can be a `System.IO.Stream`, `System.Xml.Schema.XmlSchema`, or
`System.Xml.Schema.XmlSchemaSet`.

``` C#
// From a stream (e.g. embedded resource)
Result<XDocument> result = document.Validate(schemaStream);

// From a single schema
Result<XDocument> result = document.Validate(xmlSchema);

// From a schema set
Result<XDocument> result = document.Validate(schemaSet);
```

## SchemaValidator&lt;TModel&gt;

A validator that round-trips a model through XML serialization and validates
against XSD schemas. The model type must be XML-serializable.

``` C#
var validator = new SchemaValidator<MyModel>(schemaStream);

// Validate an existing model (serializes to XML, validates, returns result)
Result<MyModel> result = validator.Validate(model);

// Validate and deserialize from an XML stream
Result<MyModel> result = validator.Deserialize(xmlStream);

// Validate and deserialize from an XML string
Result<MyModel> result = validator.Deserialize(xmlString);
```

### Schema source overloads

``` C#
// From a stream
var validator = new SchemaValidator<TModel>(schemaStream);

// From a single schema
var validator = new SchemaValidator<TModel>(xmlSchema);

// From a schema set
var validator = new SchemaValidator<TModel>(schemaSet);
```

## Building schema sets

Use the fluent `Append` method to build a schema set:

``` C#
var schemas = new XmlSchemaSet()
    .Append(schema1)
    .Append(schema2);
```

## Error messages

XML validation errors are collected as `IValidationMessage` instances with
XPath-based property names for precise error location (e.g.
`/root/element[0]/child`).
