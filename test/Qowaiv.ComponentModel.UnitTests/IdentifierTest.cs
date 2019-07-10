using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests
{
    public class IdentifierTest
    {
        [Test]
        public void Get_Null_throws()
        {
            Assert.Throws<ArgumentNullException>
            (
                () => Identifier.Get(null)
            );
        }

        [Test]
        public void Get_Object_throws()
        {
            Assert.Throws<IdentifierNotFoundException>
            (
                () => Identifier.Get(new object())
            );
        }

        [Test]
        public void Get_WithIdOfNotSupportedType_throws()
        {
            Assert.Throws<IdentifierNotFoundException>
            (
                () => Identifier.Get(new WithIdOfNotSupportedType())
            );
        }

        [Test]
        public void Get_WithKeyAttribute_SomeValue()
        {
            var model = new WithKeyAttribute { SomeProperty = "MyId" };
            var id = Identifier.Get(model);
            Assert.AreEqual("MyId", id);
        }

        [Test]
        public void Get_WithGuid_SomeGuid()
        {
            var model = new WithGuid { Id = Guid.NewGuid() };
            var id = Identifier.Get(model);
            Assert.AreEqual(model.Id, id);
        }


        [Test]
        public void Get_WithUuid_SomeGuid()
        {
            var model = new WithUuid { Id = Uuid.NewUuid() };
            var id = Identifier.Get(model);
            Assert.AreEqual(model.Id, id);
        }


        [Test]
        public void Get_WithInt32_SomeInt()
        {
            var model = new WithInt32 { Id = 17 };
            var id = Identifier.Get(model);
            Assert.AreEqual(17, id);
        }

        [Test]
        public void Get_WithInt64_SomeLong()
        {
            var model = new WithInt64 { Id = 17 };
            var id = Identifier.Get(model);
            Assert.AreEqual(17L, id);
        }
    }

    public class WithKeyAttribute
    {
        [Key]
        public string SomeProperty { get; set; }

        public Guid Id { get; set; }
    }

    public class WithGuid
    {
        public Guid Id { get; set; }
    }

    public class WithUuid
    {
        public Uuid Id { get; set; }
    }

    public class WithInt32
    {
        public int Id { get; set; }
    }

    public class WithInt64
    {
        public long Id { get; set; }
    }

    public class WithIdOfNotSupportedType
    {
        public object Id { get; set; }
    }
}
