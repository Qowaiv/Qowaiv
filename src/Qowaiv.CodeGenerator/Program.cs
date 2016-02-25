using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Qowaiv.CodeGenerator.Generators;
using Qowaiv.CodeGenerator.Generators.Svo;
using System;
using System.IO;
using System.Linq;
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

			var repository = LogManager.GetRepository() as Hierarchy;

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
		/// Example: CodeGenerator.exe C:\Temp\Qowaiv String EmailAddress "Email address" an Qowaiv
		/// </remarks>
		public static void Main(string[] args)
		{
			AppendLoggers();

			try
			{
				if (args == null || args.Length < 3) { throw new Exception("arguments required."); }

				var outputDir = new DirectoryInfo(args[0]);

				Type underlyingType = null;
				if (!args[1].Contains('.'))
				{
					underlyingType = Type.GetType("System." + args[1].Substring(0, 1).ToUpperInvariant() + args[1].Substring(1));
				}
				else
				{
					underlyingType = Type.GetType(args[1]);
				}

				if (underlyingType == null) { throw new ArgumentException("Could not resolve the underlying type."); }

				var input = new SvoStruct() { ClassName = args[2], UnderlyingType = underlyingType };

				if (args.Length > 3) { input.ClassLongName = args[3]; }
				if (args.Length > 4) { input.a = args[4].ToLower(); }
				if (args.Length > 5) { input.Namespace = args[5]; }

				var rsxGen = new QowaivCodeGenerator();
				var svoGen = new SvoStructGenerator();

				rsxGen.Generate(outputDir);
				svoGen.Generate(outputDir, input);
			   
			}
			catch(Exception x)
			{
				log.Error(x);
			}
		}
	}
}
