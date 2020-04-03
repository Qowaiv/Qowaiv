using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qowaiv.Json
{
    /// <summary>Describes how a type should be described as OpenAPI Data Type.</summary>
    /// <remarks>
    /// See: https://swagger.io/docs/specification/data-models/data-types/
    /// </remarks>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [DebuggerDisplay("{DebuggerDisplay}")]
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
            Enum = @enum?.Split(',');
        }

        /// <summary>Gets the bound .NET type of the OpenAPI Data Type.</summary>
        /// <remarks>
        /// Only set when received via one of the <c>From()</c> factory methods.
        /// </remarks>
        public Type DataType { get; internal set; }

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append($@"{{ type: {Type}");

                if (!string.IsNullOrEmpty(Format))
                {
                    sb.Append($@", format: {Format}");
                }
                if (!string.IsNullOrEmpty(Pattern))
                {
                    sb.Append($@", pattern: {Pattern}");
                }
                if (Nullable)
                {
                    sb.Append($", nullable: true");
                }
                sb.Append(" }");

                return sb.ToString();
            }
        }


        /// <summary>Gets all <see cref="OpenApiDataTypeAttribute"/>s specified in the assemblies.</summary>
        public static IEnumerable<OpenApiDataTypeAttribute> From(params Assembly[] assemblies)
        {
            Guard.NotNull(assemblies, nameof(assemblies));
            return From(assemblies.SelectMany(assembly => assembly.GetTypes()));
        }

        /// <summary>Gets all <see cref="OpenApiDataTypeAttribute"/>s of the
        /// specified types that are decorated as such.
        /// </summary>
        public static IEnumerable<OpenApiDataTypeAttribute> From(params Type[] types) => From(types?.AsEnumerable());

        /// <summary>Gets all <see cref="OpenApiDataTypeAttribute"/>s of the
        /// specified types that are decorated as such.
        /// </summary>
        public static IEnumerable<OpenApiDataTypeAttribute> From(IEnumerable<Type> types)
        {
            Guard.NotNull(types, nameof(types));

            foreach(var type in types)
            {
                var attr = type?.GetCustomAttribute<OpenApiDataTypeAttribute>();
                if(attr != null)
                {
                    attr.DataType = type;
                    yield return attr;
                }
            }
        }
    }
}
