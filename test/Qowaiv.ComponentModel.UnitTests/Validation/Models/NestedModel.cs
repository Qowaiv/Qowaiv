using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qowaiv.ComponentModel.UnitTests.Validation.Models
{
    public class NestedModel
    {
        [Mandatory]
        public Guid Id { get; set; }

        [Mandatory]
        public ChildModel Child { get; set; }

        public class ChildModel
        {
            [Mandatory]
            public string Name { get; set; }
        }

    }
}
