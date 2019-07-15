using FluentValidation;
using NUnit.Framework;
using Qowaiv.Validation.Fluent;
using System;

namespace Qowaiv.DomainModel.UnitTests
{
    public class ChildCollectionTest
    {
        [Test]
        public void Update_Valid_Applied()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.Test((p) =>
            {
                p.Numbers[1] = 666;
            });

            Assert.AreEqual(new[] { 17, 666 }, parent.Numbers);
        }

        [Test]
        public void Update_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.TestWithRollback((p) =>
            {
                p.Numbers[1] = 666;
            });
            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }


        [Test]
        public void Add_Valid_Applied()
        {
            var parent = new NumberParent();

            parent.Test((p) =>
            {
                p.Numbers.Add(17);
                p.Numbers.Add(69);
            });

            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }

        [Test]
        public void Add_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.Add(69);
            });
           
            Assert.AreEqual(new[] { 17 }, parent.Numbers);
        }


        [Test]
        public void AddRange_Valid_Applied()
        {
            var parent = new NumberParent();

            parent.Test((p) =>
            {
                p.Numbers.AddRange(new[] { 17, 69 });
            });

            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }

        [Test]
        public void AddRange_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.AddRange(new[] { 69, 666 });
            });

            Assert.AreEqual(new[] { 17 }, parent.Numbers);
        }


        [Test]
        public void Insert_Valid_Applied()
        {
            var parent = new NumberParent();

            parent.Test((p) =>
            {
                p.Numbers.Insert(0, 69);
                p.Numbers.Insert(0, 17);
                
            });

            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }

        [Test]
        public void Insert_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.Insert(0, 69);
            });

            Assert.AreEqual(new[] { 17 }, parent.Numbers);
        }


        [Test]
        public void Remove_NoneExsting_False()
        {
            var parent = new NumberParent();
            Assert.IsFalse(parent.Numbers.Remove(17));
        }

        [Test]
        public void Remove_Valid_Applied()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.Test((p) =>
            {
                p.Numbers.Remove(17);
            });

            Assert.AreEqual(new[] { 69 }, parent.Numbers);
        }

        [Test]
        public void Remove_Invalid_RolledBack()
{
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.Remove(17);
            });

            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }


        [Test]
        public void RemoveAt_Valid_Applied()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.Test((p) =>
            {
                p.Numbers.RemoveAt(0);
            });

            Assert.AreEqual(new[] { 69 }, parent.Numbers);
        }

        [Test]
        public void RemoveAt_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.RemoveAt(1);
            });

            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }

        [Test]
        public void Clear_Valid_Applied()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.Test((p) =>
            {
                p.Numbers.Clear();
            });

            Assert.AreEqual(Array.Empty<int>(), parent.Numbers);
        }

        [Test]
        public void Clear_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.Clear();
            });
            Assert.AreEqual(new[] { 17, 69 }, parent.Numbers);
        }

        [Test]
        public void Contains_SomeContainedValue_True()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            Assert.IsTrue(parent.Numbers.Contains(69));
        }

        [Test]
        public void Contains_SomeNoneContainedValue_False()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(17);

            Assert.IsTrue(parent.Numbers.Contains(69));
        }

        [Test]
        public void Sort_Valid_Applied()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(666);
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.Test((p) =>
            {
                p.Numbers.Sort();
            });

            Assert.AreEqual(new[] { 17, 69, 666 }, parent.Numbers);
        }

        [Test]
        public void Sort_Invalid_RolledBack()
        {
            var parent = new NumberParent();
            parent.Numbers.Add(666);
            parent.Numbers.Add(17);
            parent.Numbers.Add(69);

            parent.TestWithRollback((p) =>
            {
                p.Numbers.Sort();
            });

            Assert.AreEqual(new[] { 666, 17, 69 }, parent.Numbers);
        }
    }

    internal class NumberParent : AggregateRoot<NumberParent>
    {
        public NumberParent(): base(Guid.NewGuid(), new NumberParentValidator())
        {
            Numbers = new ChildCollection<int>(Tracker);
        }

        public ChildCollection<int> Numbers { get; }

        public bool Invalid { get; set; }

        public void Test(Action<NumberParent> test)
        {
            Assert.IsTrue(TrackChanges(test).IsValid);
        }

        public void TestWithRollback(Action<NumberParent> test)
        {
            Invalid = true;
            Assert.IsFalse(TrackChanges(test).IsValid);
        }

        private class NumberParentValidator : FluentModelValidator<NumberParent>
        {
            public NumberParentValidator()
            {
                RuleFor(m => m.Invalid)
                    .Must(invalid => !invalid)
                    .WithMessage("Invalid");
            }
        }
    }
}
