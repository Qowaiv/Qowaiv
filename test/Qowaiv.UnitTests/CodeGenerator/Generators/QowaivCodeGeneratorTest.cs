using NUnit.Framework;
using Qowaiv.CodeGenerator.Generators;
using Qowaiv.UnitTests.IO;

namespace Qowaiv.UnitTests.CodeGenerator.Generators
{
	[TestFixture]
	public class QowaivCodeGeneratorTest
	{
		[Test]
		public void Generate_Dir_Successful()
		{
			using (var dir = new TemporaryDirectory())
			{ 
				var gen = new QowaivCodeGenerator();
				gen.Generate(dir);
			}
		}
	}
}
