using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qowaiv.UnitTests.TestTools.CodeDom
{
    /// <summary>An assert helper class to compile (generated) code files.</summary>
    public static class CompileAssert
    {
        /// <summary>Verifies that the c# code at the specified directory compiles without errors.</summary>
        /// <param name="dir">The directory to compile.</param>
        /// <param name="references">Te assemblies to reference during compilation.</param>
        /// <remarks>
        /// Only .cs files are taken into account.
        /// </remarks>
        public static void CompilesWithoutErrors(DirectoryInfo dir, params Assembly[] references)
        {
            var files = dir.GetFiles().Where(f => f.Extension == ".cs");
            CompilesWithoutErrors(files, references);
        }

        /// <summary>Verifies that the specified files compile without errors.</summary>
        /// <param name="files">The files to compile.</param>
        /// <param name="references">Te assemblies to reference during compilation.</param>
        public static void CompilesWithoutErrors(IEnumerable<FileInfo> files, params Assembly[] references)
        {
            if (files == null || !files.Any())
            {
                throw new AssertFailedException("No files specified.");
            }

            using (var provider = new CSharpCodeProvider())
            {
                var parameters = new CompilerParameters();
                foreach (var reference in references)
                {
                    parameters.ReferencedAssemblies.Add(reference.Location);
                }

                var selected = files.Select(file => file.FullName).ToArray();
                

                var results = provider.CompileAssemblyFromFile(parameters, selected);

                foreach (var error in results.Errors.OfType<CompilerError>())
                {
                    Console.WriteLine("{0}{1}{2:0000}: {3}",
                        Path.GetFileName(error.FileName),
                        Environment.NewLine,
                        error.Line,
                        error.ErrorText);
                }
                var errorCount = results.Errors.OfType<CompilerError>().Count(e => !e.IsWarning);
                var warnCount = results.Errors.OfType<CompilerError>().Count(e => e.IsWarning);
                Assert.IsFalse(results.Errors.HasErrors, "With {0} error(s). See console output for details.", errorCount);
                Assert.IsFalse(results.Errors.HasWarnings, "With {0} warning(s). See console output for details.", warnCount);
            }
        }
    }
}
