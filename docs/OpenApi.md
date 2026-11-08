# OpenAPI Specification
The [OpenAPI Specification](https://www.openapis.org)
(formerly Swagger Specification) is an API description format for REST APIs.

To improve usage of your REST APIs you should specify the Data Type of your
SVOs. To make this as simple as possible, Qowaiv SVOs are decorated with the
`[OpenApiDataType]` attribute. It specifies the type, format, (regex) pattern,
example, and enum values, all when applicable.

If no `enum` is specified explicitly on the attribute, `OpenApiDataType.FromType()`
resolves it from the defined values of the SVO (see [Contracts](Contracts.md)).

``` json
{
  "Date": {
    "description": "Full-date notation as defined by RFC 3339, section 5.6.",
    "example": "2017-06-10",
    "type": "string",
    "format": "date"
  },
  "DateSpan": {
    "description": "Date span, specified in years, months and days.",
    "example": "1Y+10M+16D",
    "type": "string",
    "format": "date-span",
    "pattern": "[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D"
  },
  "EmailAddress": {
    "description": "Email notation as defined by RFC 5322.",
    "example": "svo@qowaiv.org",
    "type": "string",
    "format": "email"
  },
  "HouseNumber": {
    "description": "House number notation.",
    "example": "13",
    "type": "string",
    "format": "house-number"
  },
  "LocalDateTime": {
    "description": "Date-time notation as defined by RFC 3339, without time zone information.",
    "example": "2017-06-10 15:00",
    "type": "string",
    "format": "local-date-time"
  },
  "Month": {
    "description": "Month(-only) notation.",
    "example": "Jun",
    "type": "string",
    "format": "month",
    "enum": ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "?"]
  },
  "MonthSpan": {
    "description": "Month span, specified in years and months.",
    "example": "1Y+10M",
    "type": "string",
    "format": "month-span",
    "pattern": "[+-]?[0-9]+Y[+-][0-9]+M"
  },
  "Percentage": {
    "description": "Ratio expressed as a fraction of 100 denoted using the percent sign \u0027%\u0027.",
    "example": "13.76%",
    "type": "string",
    "format": "percentage",
    "pattern": "-?[0-9]+(\\.[0-9]+)?%"
  },
  "PostalCode": {
    "description": "Postal code notation.",
    "example": "2624DP",
    "type": "string",
    "format": "postal-code"
  },
  "Sex": {
    "description": "Sex as specified by ISO/IEC 5218.",
    "example": "female",
    "type": "string",
    "format": "sex",
    "enum": ["NotKnown", "Male", "Female", "NotApplicable"]
  },
  "Uuid": {
    "description": "Universally unique identifier, Base64 encoded.",
    "example": "lmZO_haEOTCwGsCcbIZFFg",
    "type": "string",
    "format": "uuid-base64"
  },
  "WeekDate": {
    "description": "Full-date notation as defined by ISO 8601.",
    "example": "1997-W14-6",
    "type": "string",
    "format": "date-weekbased"
  },
  "Year": {
    "description": "Year(-only) notation.",
    "example": 1983,
    "type": "integer",
    "format": "year"
  },
  "YearMonth": {
    "description": "Date notation with month precision.",
    "example": "2017-06",
    "type": "string",
    "format": "year-month",
    "pattern": "[0-9]{4}-(0?[1-9]|1[0-2])"
  },
  "YearSpan": {
    "description": "Year span",
    "example": 17,
    "type": "integer",
    "format": "year-span"
  },
  "YesNo": {
    "description": "Yes-No notation.",
    "example": "yes",
    "type": "string",
    "format": "yes-no",
    "enum": ["yes", "no", "?"]
  },
  "Chemistry.CasRegistryNumber": {
    "description": "CAS Registry Number",
    "example": "7732-18-5",
    "type": "string",
    "format": "cas-nr",
    "pattern": "[1-9][0-9]+\\-[0-9]{2}\\-[0-9]"
  },
  "Financial.Amount": {
    "description": "Decimal representation of a currency amount.",
    "example": 15.95,
    "type": "number",
    "format": "amount"
  },
  "Financial.BusinessIdentifierCode": {
    "description": "Business Identifier Code, as defined by ISO 9362.",
    "example": "DEUTDEFF",
    "type": "string",
    "format": "bic"
  },
  "Financial.Currency": {
    "description": "Currency notation as defined by ISO 4217.",
    "example": "EUR",
    "type": "string",
    "format": "currency"
  },
  "Financial.InternationalBankAccountNumber": {
    "description": "International Bank Account Number notation as defined by ISO 13616:2007.",
    "example": "BE71096123456769",
    "type": "string",
    "format": "iban",
    "pattern": "[A-Z]{2}[0-9]{2}[A-Z0-9]{8,32}"
  },
  "Financial.Money": {
    "description": "Combined currency and amount notation as defined by ISO 4217.",
    "example": "EUR12.47",
    "type": "string",
    "format": "money",
    "pattern": "[A-Z]{3} -?[0-9]+(\\.[0-9]+)?"
  },
  "Globalization.Country": {
    "description": "Country notation as defined by ISO 3166-1 alpha-2.",
    "example": "NL",
    "type": "string",
    "format": "country"
  },
  "IO.StreamSize": {
    "description": "Stream size notation (in byte).",
    "example": 1024,
    "type": "integer",
    "format": "stream-size"
  },
  "Mathematics.Fraction": {
    "description": "Fraction",
    "example": "13/42",
    "type": "string",
    "format": "fraction",
    "pattern": "-?[0-9]+(/[0-9]+)?"
  },
  "Statistics.Elo": {
    "description": "Elo rating system notation.",
    "example": 1600,
    "type": "number",
    "format": "elo"
  },
  "Sustainability.EnergyLabel": {
    "description": "EU energy label",
    "example": "A++",
    "type": "string",
    "format": "energy-label",
    "pattern": "A\\+{0,4}|[B-G]",
    "enum": ["G", "F", "E", "D", "C", "B", "A", "A+", "A++", "A+++", "A++++"]
  },
  "Web.InternetMediaType": {
    "description": "Media type notation as defined by RFC 6838.",
    "example": "text/html",
    "type": "string",
    "format": "internet-media-type"
  }
}
```
  
## Open API using Swagger 
When using [Swagger](https://swagger.io/resources/open-api/) to communicate
an Open API definition, this could be done like below:
``` C#
/// <summary>Extensions on <see cref="SwaggerGenOptions" />.</summary>
public static class SwaggerGenOptionsSvoExtensions
{
    /// <summary>Maps Qowaiv SVOs.</summary>
    public static SwaggerGenOptions MapSingleValueObjects(this SwaggerGenOptions options)
    {
        var infos = OpenApiDataTypes.FromAssemblies(typeof(Date).Assembly);
        foreach (var info in infos)
        {
            options.MapType(info.DataType, () => new OpenApiSchema
            {
                Type = info.Type,
                Example = info.Example(),
                Format = info.Format,
                Pattern = info.Pattern,
            });
        }
        return options;
    }

    private static IOpenApiAny Example(this OpenApiDataType info)
        => info.Example switch
        {
            int integer => new OpenApiInteger(integer),
            double floating => new OpenApiDouble(floating),
            _ => new OpenApiString(info.Example.ToString()),
        };
}
```

## Open API using Microsoft.OpenApi
When using the OpenAPI support of ASP.NET Core (`Microsoft.AspNetCore.OpenApi`,
based on `Microsoft.OpenApi` v2), the generated document can be enriched with
a schema transformer:

``` C#
builder.Services.AddOpenApi(options => options.AddSvoSchemas());

/// <summary>Extensions on <see cref="OpenApiOptions" />.</summary>
public static class OpenApiOptionsSvoExtensions
{
    /// <summary>Describes Qowaiv SVOs in the OpenAPI document.</summary>
    public static OpenApiOptions AddSvoSchemas(this OpenApiOptions options)
        => options.AddSchemaTransformer((schema, context, _) =>
        {
            var type = context.JsonTypeInfo.Type;
            var info = OpenApiDataType.FromType(Nullable.GetUnderlyingType(type) ?? type);

            if (info is not null)
            {
                // Keep "null" if the generator added it (OpenAPI 3.1: type: ["string", "null"]).
                var nullable = schema.Type.GetValueOrDefault() & JsonSchemaType.Null;

                schema.Type = info.Type switch
                {
                    "integer" => JsonSchemaType.Integer,
                    "number" => JsonSchemaType.Number,
                    _ => JsonSchemaType.String,
                } | nullable;

                schema.Description = info.Description;
                schema.Format = info.Format;
                schema.Pattern = info.Pattern;
                schema.Example = Example(info);
                schema.Properties?.Clear();

                if (info.Enum is { } values)
                {
                    schema.Enum = [.. values.Select(JsonValue.Create).OfType<JsonNode>()];
                }
            }
            return Task.CompletedTask;
        });

    private static JsonNode? Example(OpenApiDataType info) => info.Example switch
    {
        int integer => JsonValue.Create(integer),
        double floating => JsonValue.Create(floating),
        _ => JsonValue.Create(info.Example?.ToString()),
    };
}
```
