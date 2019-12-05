using System;

namespace Qowaiv.Json
{
    /// <summary>Provides custom formatting for XML serialization and deserialization.</summary>
    public interface IJsonSerializable
    {
        /// <summary>Generates an object from a JSON null object representation.</summary>
        [Obsolete("Use FromJson(object) instead.")]
        void FromJson();

        /// <summary>Generates an object from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the object.
        /// </param>
        [Obsolete("Use FromJson(object) instead.")]
        void FromJson(string jsonString);

        /// <summary>Generates an object from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the object.
        /// </param>
        [Obsolete("Use FromJson(object) instead.")]
        void FromJson(long jsonInteger);

        /// <summary>Generates an object from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the object.
        /// </param>
        [Obsolete("Use FromJson(object) instead.")]
        void FromJson(double jsonNumber);

        /// <summary>Generates an object from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the object.
        /// </param>
        [Obsolete("Use FromJson(object) instead.")]
        void FromJson(DateTime jsonDate);

        /// <summary>Generates an object from a JSON representation.</summary>
        /// <remarks>
        /// Typically JSON of the type 
        /// <see cref="string"/>, <see cref="long"/> or <see cref="double"/>.
        /// </remarks>
        void FromJson(object json);

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
