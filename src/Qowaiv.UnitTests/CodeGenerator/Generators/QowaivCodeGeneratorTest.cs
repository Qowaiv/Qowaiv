using NUnit.Framework;
using Qowaiv.CodeGenerator.Generators;
using System;
using System.IO;

namespace Qowaiv.UnitTests.CodeGenerator.Generators
{
	[TestFixture]
	public class QowaivCodeGeneratorTest
	{
		[Test]
		public void Generate_Dir_Successful()
		{
			var gen = new QowaivCodeGenerator();
			var dir = new DirectoryInfo(@"QowaivOutput");
			Console.WriteLine(dir.FullName);
			gen.Generate(dir);
		}
	}
}
