using NUnit.Framework;
using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class ChildCollectionTest
    {
        [Test]
        public void Add_Valid_Added()
        {
            var parent = new NumberParent();
            Assert.IsTrue(parent.Test((p) =>
            {
                p.Numbers.Add(17);
                p.Numbers.Add(69);
            }).IsValid);

            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }

        [Test]
        public void Add_Invalid_Added()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);

            Assert.IsFalse(parent.Test((p) =>
            {
                p.Numbers.Add(69);
                p.Invalid = true;
            }).IsValid);
           
            Assert.AreEqual(new[] { 17 }, parent.Numbers);
        }
    }

    internal class NumberParent : AggregateRoot<NumberParent>
    {
        public NumberParent()
        {
            Numbers = new ChildCollection<int>(Tracker);
        }

        public ChildCollection<int> Numbers { get; }

        public bool Invalid { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Invalid)
            {
                yield return ValidationMessage.Error("Invalid");
            }
        }

        public Result<NumberParent> Test(Action<NumberParent> test) => TrackChanges(test);
    }
}
