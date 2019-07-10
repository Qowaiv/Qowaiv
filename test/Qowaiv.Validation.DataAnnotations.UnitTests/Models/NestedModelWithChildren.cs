using System;

namespace Qowaiv.Validation.DataAnnotations.UnitTests.Models
{
    public class NestedModelWithChildren
    {
        [Mandatory]
        public Guid Id { get; set; }

        [Mandatory]
        public ChildModel[] Children { get; set; }

        [NestedModel]
        public class ChildModel
        {
            [Mandatory]
            public string Name { get; set; }
        }
    }
}
