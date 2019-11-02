﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Qowaiv.CodeGenerator.Xml
{
    /// <summary>Represents a resource file.</summary>
    [Serializable, XmlType("root")]
    public class XResourceFile
    {
        /// <summary>Initializes a new instance of a resource file.</summary>
        protected XResourceFile()
        {
            Headers = new Collection<XResourceFileHeader>();
            Data = new Collection<XResourceFileData>();
        }

        /// <summary>Initializes a new instance of a resource file.</summary>
        public XResourceFile(params XResourceFileData[] data)
        {
            Headers = new Collection<XResourceFileHeader>
            {
                XResourceFileHeader.ResMimeType,
                XResourceFileHeader.Version ,
                XResourceFileHeader.Reader,
                XResourceFileHeader.Writer
            };
            Data = new Collection<XResourceFileData>(data);
        }

        /// <summary>Initializes a new instance of a resource file.</summary>
        public XResourceFile(IEnumerable<XResourceFileData> data) : this(data.ToArray()) { }

        /// <summary>Gets the headers.</summary>
        [XmlElement(Type = typeof(XResourceFileHeader), ElementName = "resheader")]
        public Collection<XResourceFileHeader> Headers { get; private set; }

        /// <summary>Gets the data.</summary>
        [XmlElement(Type = typeof(XResourceFileData), ElementName = "data")]
        public Collection<XResourceFileData> Data { get; private set; }

        /// <summary>Gets the first (or default) item with the specified key.</summary>
        /// <param name="key">
        /// The key to search for.
        /// </param>
        public XResourceFileData this[string key] => Data.FirstOrDefault(data => data.Name == key);

        /// <summary>Saves the resource file to a file.</summary>
        /// <param name="file">
        /// The file to safe to.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use.
        /// </param>
        public void Save(FileInfo file, Encoding encoding = null)
        {
            Guard.NotNull(file, "file");
            using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
            {
                Save(stream);
            }
        }

        /// <summary>Saves the resource file to a stream.</summary>
        /// <param name="stream">
        /// The stream to safe to.
        /// </param>
        /// <param name="encoding">
        /// The encoding to use.
        /// </param>
        public void Save(Stream stream, Encoding encoding = null)
        {
            Guard.NotNull(stream, "stream");
            serializer.Serialize(stream, this);
        }

        /// <summary>Loads a resource file from stream.</summary>
        /// <param name="stream">
        /// The stream to load from.
        /// </param>
        public static XResourceFile Load(Stream stream)
        {
            Guard.NotNull(stream, "stream");
            return (XResourceFile)serializer.Deserialize(stream);
        }

        /// <summary>Loads a resource file from stream.</summary>
        /// <param name="file">
        /// The file to load from.
        /// </param>
        public static XResourceFile Load(FileInfo file)
        {
            Guard.NotNull(file, "file");
            using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                return Load(stream);
            }
        }
        /// <summary>Loads a resource file from stream.</summary>
        /// <param name="file">
        /// The file to load from.
        /// </param>
        public static XResourceFile Load(string file) => Load(new FileInfo(file));

        /// <summary>The serializer to load and save the resource file.</summary>
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(XResourceFile), string.Empty);
    }
}
