using Qowaiv.CodeGenerator.CodeDom.Razor;
using Qowaiv.CodeGenerator.Generators.Svo;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Qowaiv.CodeGenerator.Generators
{
    /// <summary>A Single Value Object Struct generator.</summary>
    public class SvoStructGenerator
    {
        /// <summary>Gets the templates used to generate the Single Value Objects.</summary>
        public static RazorTemplates<RazorTemplate> Templates
        {
            get
            {
                if (s_Templates == null)
                {
                    var names = typeof(SvoStructGenerator).Assembly
                        .GetManifestResourceNames()
                        .Where(name => name.EndsWith(".cshtml")).ToList();

                    var streams = names.Select(name=>
                    {
                        var stream = typeof(SvoStructGenerator).Assembly.GetManifestResourceStream(name);

                        var index = name.IndexOf('1');
                        var nodots = name.Substring(index+1, name.LastIndexOf('.'));

                        while (nodots.Contains('.'))
                        {
                            nodots = nodots.Substring(nodots.IndexOf('.') + 1);
                        }
                        return new TemplateStream(stream, nodots);
                    }).ToList();

                    s_Templates = new RazorTemplates<RazorTemplate>();
                    s_Templates.Initialize(streams, new List<Assembly>()
                    {
                        typeof(System.Int32).Assembly,
                        typeof(System.Linq.Enumerable).Assembly,
                        typeof(Qowaiv.CodeGenerator.CodeDom.Razor.RazorTemplate).Assembly,
                    },
                    "Qowaiv.CodeGenerator.Generators.Svo");
                }
                return s_Templates;
            }
        }
        private static RazorTemplates<RazorTemplate> s_Templates = null;

        /// <summary>Generates code for the Single Value Object.</summary>
        /// <param name="dir">
        /// The directory to write the files to.
        /// </param>
        /// <param name="input">
        /// The input data of the Single Value Object.
        /// </param>
        public void Generate(DirectoryInfo dir, SvoStruct input)
        {
            var fileStruct = new FileInfo(Path.Combine(dir.FullName, input.ClassName + ".cs"));
            var fileJavaScript = new FileInfo(Path.Combine(dir.FullName, "Qowaiv." + input.ClassName + ".js"));
            var fileTypeConverter = new FileInfo(Path.Combine(dir.FullName, input.ClassName + "TypeConverter.cs"));
            var fileUnitTest = new FileInfo(Path.Combine(dir.FullName, input.ClassName + "Test.cs"));

            var templateStruct = Templates.GetTemplate(typeof(SvoStruct));
            var templateJavaScript = Templates.GetTemplate(typeof(SvoStruct), "_JavaScript");
            var templateTypeConverter = Templates.GetTemplate(typeof(SvoStruct), "_TypeConverter");
            var templateUnitTest = Templates.GetTemplate(typeof(SvoStruct), "_UnitTest");

            templateStruct.GenerateFile(input, fileStruct);
            templateJavaScript.GenerateFile(input, fileJavaScript);
            templateTypeConverter.GenerateFile(input, fileTypeConverter);
            templateUnitTest.GenerateFile(input, fileUnitTest);
        }
    }
}
