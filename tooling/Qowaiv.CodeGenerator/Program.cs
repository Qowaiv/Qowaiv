using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Qowaiv.CodeGenerator.Generators;
using Qowaiv.CodeGenerator.Generators.Svo;
using System;
using System.IO;
using System.Text;

namespace Qowaiv.CodeGenerator
{
    /// <summary>The code generator program.</summary>
    public static class Program
    {
        /// <summary>Logger for the collector.</summary>
        private static ILog log = LogManager.GetLogger(typeof(Program));

        private static void AppendLoggers()
        {
            var fileAppender = new FileAppender()
            {
                File = "../Qowaiv.CodeGenerator.log",
                AppendToFile = true,
                Encoding = Encoding.UTF8,
                ImmediateFlush = true,
                Name = "FileAppender",
                Layout = new PatternLayout("%d{HH:mm:ss.fff} %-5p %c - %m%n"),
            };
            fileAppender.ActivateOptions();

            var consoleAppender = new ConsoleAppender()
            {
                Name = "ConsoleAppender",
                Layout = new PatternLayout("%d{HH:mm:ss} %-5p %m%n"),
            };
            consoleAppender.ActivateOptions();

            var repository = LogManager.GetRepository(typeof(Program).Assembly) as Hierarchy;

            if (repository.Root.GetAppender("FileAppender") == null)
            {
                repository.Root.AddAppender(fileAppender);
            }
            if (repository.Root.GetAppender("ConsoleAppender") == null)
            {
                repository.Root.AddAppender(consoleAppender);
            }
            //configure the logging at the root.
            repository.Root.Level = log4net.Core.Level.Info;

            //mark repository as configured and
            //notify that is has changed.
            repository.Configured = true;
            repository.RaiseConfigurationChanged(EventArgs.Empty);
        }

        /// <summary>Executes the program.</summary>
        /// <param name="args">The Arguments of the program.</param>
        /// <remarks>
        /// CodeGenerator.exe outputDir underlyingType className [longClassName] [a|an] [namespace]
        /// 
        /// Example: CodeGenerator.exe C:\Temp\Qowaiv string EmailAddress "Email address" an Qowaiv
        /// </remarks>
        [STAThread]
        public static void Main(string[] args)
        {
            AppendLoggers();

            try
            {
                if (args == null || args.Length < 3) { throw new ArgumentException("arguments required."); }

                var outputDir = new DirectoryInfo(args[0]);

                Type underlyingType = GetType(args[1]);

                var input = new SvoStruct() { ClassName = args[2], UnderlyingType = underlyingType };

                if (args.Length > 3) { input.ClassLongName = args[3]; }
                if (args.Length > 4) { input.a = args[4].ToLower(); }
                if (args.Length > 5) { input.Namespace = args[5]; }

                var rsxGen = new QowaivCodeGenerator();
                var svoGen = new SvoStructGenerator();

                rsxGen.Generate(outputDir);
                svoGen.Generate(outputDir, input);
            }
            catch (Exception x)
            {
                log.Error(x);
            }
        }

        private static Type GetType(string name)
        {
            var typeName = name.Contains(".") ? name : "System." + name.Substring(0, 1).ToUpperInvariant() + name.Substring(1);
            var type = Type.GetType(typeName);
            return type ?? throw new ArgumentException("Could not resolve the underlying type.");
        }
    }
}
