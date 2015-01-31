using log4net;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
	/// <summary>Represents a collection of razor templates.</summary>
	/// <typeparam name="T">
	/// The type of the razor templates.
	/// </typeparam>
	public class RazorTemplates<T> : IRazorTemplateRendering where T : RazorTemplate
	{
		private static Assembly QowaivCodeDom = typeof(RazorTemplate).Assembly;

		private static ILog log = LogManager.GetLogger("Qowaiv.CodeGenerator.CodeDom.Razor.RazorTemplates");

		/// <summary>Initializes a new collection of razor templates.</summary>
		public RazorTemplates()
		{
			m_Templates = new List<T>();
		}

		/// <summary>Gets the templates.</summary>
		public ICollection<T> Templates { get { return m_Templates; } }
		/// <summary>The underlying property of the list of templates.</summary>
		private List<T> m_Templates;

		/// <summary>Gets a razor template.</summary>
		/// <param name="name">
		/// The name of the template to search for.
		/// </param>
		public T GetTemplate(string name)
		{
			Guard.NotNull(name, "name");

			var template = this.Templates.FirstOrDefault(t => t.Name == name);
			if (template == null)
			{
				throw new TemplateNotFoundException(name);
			}
			return template;
		}

		/// <summary>Gets a razor template.</summary>
		/// <param name="type">
		/// The type to search a template for.
		/// </param>
		/// <param name="postfix">
		/// The postfix for the template name.
		/// </param>
		public T GetTemplate(Type type, string postfix = null)
		{
			var searchType = Guard.NotNull(type, "type");
			T template = default(T);

			// search for this type and its basetypes.
			while (template == null && searchType != null)
			{
				var name = GetTemplateName(searchType, postfix);
				template = this.Templates.FirstOrDefault(t => t.Name == name);
				searchType = searchType.BaseType;
			}
			if (template == null)
			{
				throw new TemplateNotFoundException(type, postfix);
			}
			return template;
		}

		/// <summary>Renders a model and returns the string result.</summary>
		/// <param name="model">
		/// The model to render.
		/// </param>
		/// <param name="postfix">
		/// </param>
		public string RenderModel(object model, string postfix = null)
		{
			T template = GetTemplate(model.GetType(), postfix);

			using (var writer = new StringWriter(CultureInfo.InvariantCulture))
			{
				template.WriteToTextWriter(model, writer);
				return writer.ToString();
			}
		}

		/// <summary>Renders a snippet.</summary>
		/// <param name="name">
		/// The name of the snippet.
		/// </param>
		public string RenderSnippet(string name)
		{
			T template = GetTemplate(name);

			using (var writer = new StringWriter(CultureInfo.InvariantCulture))
			{
				template.Execute();
				writer.Write(template.Buffer.ToString());
				template.Buffer.Clear();
				return writer.ToString();
			}
		}

		/// <summary>Constructs a template name based on the type and the postfix.</summary>
		protected virtual string GetTemplateName(Type type, string postfix)
		{
			return type.Name + postfix;
		}

		/// <summary>Gets the name for the template.</summary>
		protected virtual void SetTemplateName(RazorTemplate razorTemplate)
		{
			var name = razorTemplate.GetType().Name;
			name = name.Substring(0, name.Length - "RazorTemplate".Length);
			razorTemplate.Name = name;
		}

		/// <summary>Initializes razor templates from an assembly.</summary>
		/// <param name="templatestreams">
		/// The streams to initializes.
		/// </param>
		/// <param name="referencedAssemblies">
		/// The assembies 
		/// </param>
		/// <param name="defaultNamespace">
		/// The default namespace.
		/// </param>
		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Initialize(IEnumerable<TemplateStream> templatestreams, IEnumerable<Assembly> referencedAssemblies, string defaultNamespace = "Qowaiv.CodeGenerator.CodeDom.Razor")
		{
			Assembly templateAssembly = null;

			var genResults = new List<GeneratorResults>();
			
			foreach (var templatestream in templatestreams)
			{
			var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
				var razorEngine = new RazorTemplateEngine(host);

				host.DefaultBaseClass = typeof(T).FullName;
				// make namespace and skip the root sign.
				host.DefaultNamespace = defaultNamespace;

				host.DefaultClassName = templatestream.ClassName + "RazorTemplate";

				var reader = new StreamReader(templatestream.Stream);

				using (var provider = new CSharpCodeProvider())
				{
					var genResult = razorEngine.GenerateCode(reader);

					foreach (var error in genResult.ParserErrors)
					{
						log.Error(error);
					}
					genResults.Add(genResult);
				}
			}

			log.Info("Start compiling template dll.");

			using (var provider = new CSharpCodeProvider())
			{
				var parameters = new CompilerParameters();
				parameters.IncludeDebugInformation = true;
				parameters.GenerateInMemory = true;

				foreach (var assembly in referencedAssemblies)
				{
					log.DebugFormat("Add assembly {0}", assembly);
					parameters.ReferencedAssemblies.Add(assembly.Location);
				}
				if (!referencedAssemblies.Contains(QowaivCodeDom))
				{
					log.DebugFormat("Add assembly {0}", QowaivCodeDom);
					parameters.ReferencedAssemblies.Add(QowaivCodeDom.Location);
				}

				var dll = provider.CompileAssemblyFromDom(parameters, genResults.Select(item => item.GeneratedCode).ToArray());
				foreach (var file in dll.TempFiles)
				{
					log.Info(file);
				}

				if (dll.Errors.HasErrors)
				{
					log.Error("Errors found during compiling template assembly.");
					foreach (var error in dll.Errors.OfType<CompilerError>())
					{
						if (error.IsWarning)
						{
							log.Warn(error);
						}
						else
						{
							log.Error(error);
						}
					}
				}
				else
				{
					templateAssembly = dll.CompiledAssembly;
				}
			}

			// Creates an instance for each type and add it to the collection.
			if (templateAssembly != null)
			{
				m_Templates.AddRange(templateAssembly.GetTypes().Select(tp => Activator.CreateInstance(tp)).Cast<T>());
			}

			// Adds a reference from template to collection and sets the name.
			foreach (var template in this.m_Templates)
			{
				template.Collection = this;
				SetTemplateName(template);
				log.InfoFormat("Add template '{0}' to collection.", template.Name);
			}
		}
	}
}
