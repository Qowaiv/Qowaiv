using Qowaiv.CodeGenerator.CodeDom.Razor;
using Qowaiv.CodeGenerator.Generators.Svo;
using System.IO;

namespace Qowaiv.CodeGenerator.Generators
{
    /// <summary>A Single Value Object Struct generator.</summary>
    public class SvoStructGenerator
    {
        /// <summary>Generates code for the Single Value Object.</summary>
        /// <param name="dir">
        /// The directory to write the files to.
        /// </param>
        /// <param name="input">
        /// The input data of the Single Value Object.
        /// </param>
        public void Generate(DirectoryInfo dir, SvoStruct input)
        {
            Guard.NotNull(dir, nameof(dir));
            Guard.NotNull(input, nameof(input));

            var fileStruct = new FileInfo(Path.Combine(dir.FullName, input.ClassName + ".cs"));
            var fileJavaScript = new FileInfo(Path.Combine(dir.FullName, "Qowaiv." + input.ClassName + ".js"));
            var fileTypeConverter = new FileInfo(Path.Combine(dir.FullName, input.ClassName + "TypeConverter.cs"));
            var fileUnitTest = new FileInfo(Path.Combine(dir.FullName, input.ClassName + "Test.cs"));

            var templateStruct = RazorTemplates.Select<SvoStruct>();
            var templateJavaScript = RazorTemplates.Select<SvoStruct>("_JavaScript");
            var templateTypeConverter = RazorTemplates.Select<SvoStruct>("_TypeConverter");
            var templateUnitTest = RazorTemplates.Select<SvoStruct>("_UnitTest");

            templateStruct.GenerateFile(input, fileStruct);
            templateJavaScript.GenerateFile(input, fileJavaScript);
            templateTypeConverter.GenerateFile(input, fileTypeConverter);
            templateUnitTest.GenerateFile(input, fileUnitTest);
        }
    }
}
