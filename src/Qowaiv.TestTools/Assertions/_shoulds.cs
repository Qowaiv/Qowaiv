using Qowaiv.OpenApi;
using Qowaiv.TestTools.Assertions;

namespace Qowaiv.TestTools;

/// <summary>Asserhtions helpers AwesomeAssertions style.</summary>
public static class Shoulds
{
    extension(OpenApiDataType? subject)
    {
        /// <summary>Returns <see cref="OpenApiDataTypeAssertions"/> for specialized assertions on <see cref="OpenApiDataType"/>.</summary>
        [Pure]
        public OpenApiDataTypeAssertions Should() => new(subject);
    }
}
