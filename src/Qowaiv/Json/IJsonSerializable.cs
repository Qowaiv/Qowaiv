using System;

namespace Qowaiv.Json
{
    /// <summary>Provides custom formatting for XML serialization and deserialization.</summary>
    [Obsolete("IJsonSerializable is not longer supported. Use a convention based serializer instead.", true)]
    public interface IJsonSerializable
    {
        /// <summary>Generates an object from a JSON null representation.</summary>
        void FromJson();

        /// <summary>Generates an object from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the object.
        /// </param>
        void FromJson(string jsonString);

        /// <summary>Generates an object from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the object.
        /// </param>
        void FromJson(long jsonInteger);

        /// <summary>Generates an object from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the object.
        /// </param>
        void FromJson(double jsonNumber);

        /// <summary>Generates an object from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the object.
        /// </param>
        void FromJson(DateTime jsonDate);

        /// <summary>Converts an object into its JSON object representation.</summary>
        /// <remarks>
        /// Typically an object of the type 
        /// <see cref="string"/>,
        /// System.Int64, 
        /// System.Double or 
        /// System.DateTime.
        /// </remarks>
        object ToJson();
    }
}
