using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.Financial;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
    public class ModelWithCustomizedResource
    {
        [Mandatory(ErrorMessageResourceType = typeof(TestMessages), ErrorMessageResourceName = "TestError")]
        [Display(Name = "IBAN")]
        public InternationalBankAccountNumber Iban { get; set; }
    }
}
