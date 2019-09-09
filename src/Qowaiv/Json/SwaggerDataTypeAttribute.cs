using System;

namespace Qowaiv.Json
{
    /// <summary>Describes how a type should be described as Swagger Data Type.</summary>
    /// <remarks>
    /// See: https://swagger.io/docs/specification/data-models/data-types/
    /// </remarks>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SwaggerDataTypeAttribute: Attribute
    {
        /// <summary>Creates a new instance of a <see cref="SwaggerDataTypeAttribute"/>.</summary>
        public SwaggerDataTypeAttribute(string type = "string", string format = null, bool nullable = false, string pattern = null)
        {
            Type = Guard.NotNullOrEmpty(type, nameof(type));
            Format = format;
            Nullable = nullable;
            Pattern = pattern;
        }

        /// <summary>Gets the type of the Swagger Data Type.</summary>
        public string Type { get; }
        
        /// <summary>Gets the format of the Swagger Data Type.</summary>
        public string Format { get; }
        
        /// <summary>Gets if the Swagger Data Type is nullable or not.</summary>
        public bool Nullable { get; }

        /// <summary>Gets the Pattern of the Swagger Data Type.</summary>
        public string Pattern { get; }
    }
}
