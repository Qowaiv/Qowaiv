using Qowaiv.ComponentModel.DataAnnotations;
using System;

namespace Qowaiv.Validation.DataAnnotations.UnitTests.Models
{
    public class NestedModel
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
        }

    }
}
