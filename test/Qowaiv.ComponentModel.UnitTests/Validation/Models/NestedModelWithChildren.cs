using Qowaiv.ComponentModel.DataAnnotations;
using System;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
    public class NestedModelWithChildren
    {
        [Mandatory]
        public Guid Id { get; set; }

        [Mandatory]
        public ChildModel[] Children { get; set; }

        public class ChildModel
        {
            [Mandatory]
            public string Name { get; set; }
        }
    }
}
