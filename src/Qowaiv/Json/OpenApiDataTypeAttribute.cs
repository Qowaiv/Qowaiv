using System;
using System.Text;

namespace Qowaiv.Json
{
    /// <summary>Describes how a type should be described as OpenAPI Data Type.</summary>
    /// <remarks>
    /// See: https://swagger.io/docs/specification/data-models/data-types/
    /// </remarks>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class OpenApiDataTypeAttribute : Attribute
    {
        /// <summary>Creates a new instance of a <see cref="OpenApiDataTypeAttribute"/>.</summary>
        public OpenApiDataTypeAttribute(
            string description,
            string type, 
            string format = null,
            bool nullable = false,
            string pattern = null,
            string @enum = null)
        {
            Description = Guard.NotNullOrEmpty(description, nameof(description));
            Type = Guard.NotNullOrEmpty(type, nameof(type));
            Format = format;
            Nullable = nullable;
            Pattern = pattern;
            Enum = @enum is null ? null : @enum.Split(',');
        }

        /// <summary>Gets the description of the OpenAPI Data Type.</summary>
        public string Description { get; }

        /// <summary>Gets the type of the OpenAPI Data Type.</summary>
        public string Type { get; }

        /// <summary>Gets the format of the OpenAPI Data Type.</summary>
        public string Format { get; }

        /// <summary>Gets if the OpenAPI Data Type is nullable or not.</summary>
        public bool Nullable { get; }

        /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
        public string Pattern { get; }

        /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
        public string[] Enum { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($@"{{ type: ""{Type}""");

            if (!string.IsNullOrEmpty(Format))
            {
                sb.Append($@", format: ""{Format}""");
            }

            if (!string.IsNullOrEmpty(Pattern))
            {
                sb.Append($@", pattern: ""{Pattern}""");
            }

            if (Nullable)
            {
                sb.Append($", nullable: true");
            }

            sb.Append(" }");
            return sb.ToString();
        }
    }
}
