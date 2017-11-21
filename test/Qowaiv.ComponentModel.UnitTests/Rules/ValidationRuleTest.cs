using NUnit.Framework;
using Qowaiv.Globalization;

namespace Qowaiv.ComponentModel.UnitTests.Rules
{
    public class ValidationRuleTest
    {
        [Test]
        public void Validate_DummyRule_CustomizedDutchMessage()
        {
            using (new CultureInfoScope("nl-NL"))
            {
                var rule = new DummyRule();
                var act = rule.Validate(null).ToString();
                var exp = "Dummy is fout.";
                Assert.AreEqual(exp, act);
            }
        }
    }
}
