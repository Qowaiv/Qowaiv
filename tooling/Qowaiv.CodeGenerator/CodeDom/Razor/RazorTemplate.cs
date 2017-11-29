using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System.Diagnostics;
using System.IO;

namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    /// <summary>Represents a template stream.</summary>
    public class RazorTemplate<T> : TemplateBase<T>
    {
        /// <summary>Initializes a new instance of a template stream.</summary>
        /// <param name="stream">
        /// The stream of the template.
        /// </param>
        /// <param name="classname">
        /// The class name of the template.
        /// </param>
        public RazorTemplate(Stream stream, string classname)
        {
            using (var reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }
            ClassName = classname;
        }

        /// <summary>Gets the class name.</summary>
        public string ClassName { get; }

        public override string ToString() => data;

        /// <summary>Gets the stream.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string data;

        public void GenerateFile(T model, FileInfo targetLocation)
        {
            var config = new TemplateServiceConfiguration
            {
                Language = Language.CSharp,
                Debug = true,
                EncodedStringFactory = new RawStringFactory()

            };

            var service = RazorEngineService.Create(config);
            using (var writer = new StreamWriter(targetLocation.FullName, true))
            {
                var result = service.RunCompile(data, ClassName, typeof(T), model);
                writer.Write(result);
                writer.Flush();
            }
        }
    }
}
