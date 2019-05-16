using Qowaiv.ComponentModel.DataAnnotations;
using System;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
    public class NestedModelWithLoop
    {
        [Mandatory]
        public Guid Id { get; set; }

        [Mandatory]
        public ChildModel Child { get; set; }

        [NestedModel]
        public class ChildModel
        {
            [Mandatory]
            public string Name { get; set; }

            public NestedModelWithLoop Parent { get; set; }
        }
    }
}
