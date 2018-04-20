using Qowaiv.ComponentModel.Rules;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.Rules
{
    public class DummyRule : ValidationRule
    {
        public DummyRule(): base("Dummy")
        {
            ErrorMessageResourceType = typeof(TestMessages);
            ErrorMessageResourceName = "TestError";
        }
        protected override bool IsValid(ValidationContext validationContext) => false;
    }
}
