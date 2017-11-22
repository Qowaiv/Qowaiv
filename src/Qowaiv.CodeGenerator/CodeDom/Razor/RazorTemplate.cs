using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    /// <summary>Represents a razor template.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class RazorTemplate
    {
        /// <summary>Initializes a new razor template.</summary>
        protected RazorTemplate()
        {
            Buffer = new StringBuilder();
            Writer = new StringWriter(Buffer, CultureInfo.InvariantCulture);
        }

        /// <summary>Gets and sets the Name of the tempate.</summary>
        public virtual string Name { get; set; }

        /// <summary>Gets and sets the Buffer of the tempate.</summary>
        [Browsable(false)]
        public StringBuilder Buffer { get; set; }

        /// <summary>Gets and sets the Writer of the tempate.</summary>
        [Browsable(false)]
        public StringWriter Writer { get; set; }

        /// <summary>Sets the Model of the template.</summary>
        /// <param name="val">
        /// The value to set.
        /// </param>
        public virtual void SetModel(object val)
        {
            var model = GetType().GetProperty("Model");
            if (model != null)
            {
                model.SetValue(this, val, null);
            }
        }

        /// <summary>Executes the template.</summary>
        public abstract void Execute();

        /// <summary>Writes a value.</summary>
        /// <remarks>
        /// Writes the results of expressions like: "@foo.Bar".
        /// </remarks>
        public virtual void Write(object value)
        {
            // Don't need to do anything special
            // Razor for ASP.Net does HTML encoding here.
            WriteLiteral(value);
        }

        /// <summary>Writes a literal value.</summary>
        /// <remarks>
        /// Writes literals like markup: "<p>Foo</p>".
        /// </remarks>
        public virtual void WriteLiteral(object value)
        {
            Buffer.Append(value);
        }

        /// <summary>A collection with all available templates.</summary>
        public IRazorTemplateRendering Collection { get; set; }

        /// <summary>Renders a model.</summary>
        /// <param name="model">
        /// The model to render.
        /// </param>
        /// <param name="postfix">
        /// The optional postfix for finding the template.
        /// </param>
        public string RenderModel(object model, string postfix = null) => Collection.RenderModel(model, postfix);

        /// <summary>Renders a snippet.</summary>
        /// <param name="name">
        /// The name of the snippet.
        /// </param>
        public string RenderSnippet(string name) => Collection.RenderSnippet(name);

        /// <summary>Generates a template and writes it to a file.</summary>
        /// <param name="model">
        /// The model to feed to the template.
        /// </param>
        /// <param name="file">
        /// The result file.
        /// </param>
        public void GenerateFile(object model, FileInfo file)
        {
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            using (var writer = new StreamWriter(file.FullName, false, Encoding.UTF8))
            {
                WriteToTextWriter(model, writer);
            }
        }

        /// <summary>Generates a template and writes it to a file.</summary>
        /// <param name="model">
        /// The model to feed to the template.
        /// </param>
        /// <param name="dir">
        /// The result directory.
        /// </param>
        /// <param name="path">
        /// The result path relative to the directory.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "The 'dir' represents an directory, not an file.")]
        public void GenerateFile(object model, DirectoryInfo dir, string path) => GenerateFile(model, new FileInfo(Path.Combine(dir.FullName, path)));

        /// <summary>Generates a template and writes it to a text writer.</summary>
        /// <param name="model">
        /// The model to feed to the template.
        /// </param>
        /// <param name="writer">
        /// The Writer to write to.
        /// </param>
        public void WriteToTextWriter(object model, TextWriter writer)
        {
            SetModel(model);
            Execute();
            writer.Write(Buffer.ToString());
            Buffer.Clear();
        }

        /// <summary>Represents the template as string.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "Template: {1}, Type: {0}",
                    GetType(),
                    Name);
            }
        }
    }
}
