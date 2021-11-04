using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Hashing;
using System;

namespace Hashing.Hash_specs
{
    public class Get_hash_code
    {
        [Test]
        public void Null_String_is_supported()
        {
            using (Hash.WithFixedRandomizer())
            {
                string nil = null;
                var hash = Hash.Code(nil);
                hash.Should().Be(740020621);
            }
        }
    }
    public class Get_hash_code_with_fixed_randomizer
    {
        [Test]
        public void String_is_fixed()
        {
            using(Hash.WithFixedRandomizer())
            {
                var hash = Hash.Code("QOWAIV string");
                hash.Should().Be(1211348473);
            }
        }

        [Test]
        public void Int32_is_fixed()
        {
            using (Hash.WithFixedRandomizer())
            {
                var hash = Hash.Code(17);
                hash.Should().Be(20170594);
            }
        }

        [Test]
        public void Int64_is_fixed()
        {
            using (Hash.WithFixedRandomizer())
            {
                var hash = Hash.Code(12345678909876L);
                hash.Should().Be(1929223677);
            }
        }
    }

    public class Get_hash_code_without_fixed_randomizer
    {
        [Test]
        public void String_is_different_from_fixed()
        {
            int fix;
            using (Hash.WithFixedRandomizer())
            {
                fix = Hash.Code("QOWAIV string");
            }
            fix.Should().NotBe(Hash.Code("QOWAIV string"));
        }

        [Test]
        public void Int32_is_different_from_fixed()
        {
            int fix;
            using (Hash.WithFixedRandomizer())
            {
                fix = Hash.Code(17);
            }
            fix.Should().NotBe(Hash.Code(17));
        }

        [Test]
        public void Int64_is_different_from_fixed()
        {
            int fix;
            using (Hash.WithFixedRandomizer())
            {
                fix = Hash.Code(12345678909876L);
            }
            fix.Should().NotBe(Hash.Code(1712345678909876L));
        }

        [Test]
        public void String_gives_same_result()
        {
            var first = Hash.Code("QOWAIV string");
            first.Should().Be(Hash.Code("QOWAIV string"));
        }

        [Test]
        public void Int32_gives_same_result()
        {
            var first = Hash.Code(17);
            first.Should().Be(Hash.Code(17));
        }

        [Test]
        public void Int64_gives_same_result()
        {
            var first = Hash.Code(1712345678909876L);
            first.Should().Be(Hash.Code(1712345678909876L));
            Console.WriteLine(first);
        }
    }

    public class Not_supported
    {
        [Test]
        public void for_type_throws_exception()
        {
            Func<int> hash = () => Hash.NotSupportedBy<string>();
            hash.Should().Throw<HashingNotSupported>()
                .WithMessage("Hashing is not supported by System.String.");
        }
    }
}
