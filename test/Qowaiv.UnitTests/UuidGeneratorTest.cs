using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qowaiv.UnitTests
{
    public class UuidGeneratorTest
    {
        private const int MultipleCount = 10000;

        internal DateTime MaxDate
        {
            get
            {
                var ticks = (0xFF_FFFF_FFFF_FFFF << 5) + 0x89F_7FF5_F7B5_8000;
                return new DateTime(ticks);
            }
        }

        [Test]
        public void Ticks1970()
        {
            var ticks = new DateTime(1970, 01, 01).Ticks;
            Assert.AreEqual("89F7FF5F7B58000", ticks.ToString("X2"));
        }

        [Test]
        public void Squential_MaxDate_Year9276()
        {
            var expected = new DateTime(9276, 12, 03, 18, 42, 01).AddTicks(3693920);
            Assert.AreEqual(expected, MaxDate);
        }

        /// <remarks>Due to the reordening the version ands up in index 7.</remarks>
        [Test]
        public void Sequential_MinDate_First6BytesAre0()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(1970, 01, 01)))
            {
                var actual = Uuid.NewSequential();
                var expected = new byte?[]
                {
                    0, 0, 0, 0,

                    0, 0, null, 0x60,
                };
                AssertBytes(expected, actual);
            }
        }

        [Test]
        public void Sequential_SqlServerMinDate_First7BytesAre0()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(1970, 01, 01)))
            {
                var actual = Uuid.NewSequential(UuidComparer.SqlServer);
                var expected = new byte?[]
                {
                    null, null, null, null,

                    null, null, null, null,

                    0, null, 0, 0,

                    0, 0, 0, 0,
                };
                AssertBytes(expected, actual);
            }
        }

        /// <remarks>Due to the reordening the version ands up in index 7.</remarks>
        [Test]
        public void Sequential_MaxDate_First7BytesAre255()
        {
            using (Clock.SetTimeForCurrentThread(() => MaxDate))
            {
                var actual = Uuid.NewSequential();
                var expected = new byte?[]
                {
                    0xFF, 0xFF, 0xFF, 0xFF,

                    0xFF, 0xFF, null, 0x6F,
                };
                AssertBytes(expected, actual);
            }
        }

        [Test]
        public void Sequential_SqlServerMaxDate_First7BytesAre0()
        {
            using (Clock.SetTimeForCurrentThread(() => MaxDate))
            {
                var actual = Uuid.NewSequential(UuidComparer.SqlServer);
                var expected = new byte?[]
                {
                    null, null, null, null,

                    null, null, null, null,

                    0xFF, null, 0xFF, 0xFF,

                    0xFF, 0xFF, 0xFF, 0xFF,
                };
                AssertBytes(expected, actual);
            }
        }

        [Test]
        public void Sequential_Multiple_IsOrdered() => AssertMultipleSequential(UuidComparer.Default);

        [Test]
        public void Sequential_MultipleMongoDb_IsOrdered() => AssertMultipleSequential(UuidComparer.MongoDb);

        [Test]
        public void Sequential_MultipleSqlServer_IsOrdered() => AssertMultipleSequential(UuidComparer.SqlServer);

        private static void AssertMultipleSequential(UuidComparer comparer)
        {
            var ids = new List<Uuid>(MultipleCount);

            foreach (var date in GetTimes().Take(MultipleCount))
            {
                using (Clock.SetTimeForCurrentThread(() => date))
                {
                    var id = Uuid.NewSequential(comparer);
                    Console.WriteLine(ToString(id, comparer));
                    ids.Add(id);
                }
            }
            CollectionAssert.IsOrdered(ids, comparer);
        }

        private static void AssertBytes(byte?[] expected, Uuid actual)
        {
            var act = new List<string>();
            var exp = new List<string>();
            var fail = false;

            var bytes = actual.ToByteArray();

            for (var i = 0; i < bytes.Length; i++)
            {
                var a = bytes[i].ToString("X2");

                if (i < expected.Length && expected[i].HasValue)
                {
                    var e = expected[i].Value.ToString("X2");

                    if (e == a)
                    {
                        act.Add(a);
                        exp.Add(e);
                    }
                    else
                    {
                        act.Add('[' + a + ']');
                        exp.Add('[' + e + ']');
                        fail = true;
                    }
                }
                else
                {
                    act.Add(a);
                    exp.Add("..");
                }
            }

            if (fail)
            {
                Assert.Fail($@"Expected: [{(string.Join(", ", exp))}]
Actual:   [{(string.Join(", ", act))}]");
            }

            Assert.AreEqual(UuidVersion.Sequential, actual.Version);
        }

        private static IEnumerable<DateTime> GetTimes()
        {
            var i = 17;

            var date = new DateTime(1970, 01, 01);

            while(true)
            {
                date = date.AddSeconds(3).AddTicks(i++);
                yield return date;
            }
        }

        private static string ToString(Uuid uuid, UuidComparer comparer)
        {
            var bytes = uuid.ToByteArray();
            var sb = new StringBuilder(48);

            foreach (var index in comparer.Priority)
            {
                sb.Append(bytes[index].ToString("X2")).Append(" ");
            }
            return sb.ToString();
        }
        
    }
}
