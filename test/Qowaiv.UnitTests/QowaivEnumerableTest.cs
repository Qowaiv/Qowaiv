using NUnit.Framework;
using Qowaiv.Financial;
using System;
using System.Linq;

namespace Qowaiv.UnitTests
{
    public class QowaivEnumerableTest
    {
        private static readonly Amount[] Amounts = new Amount[] { (Amount)1, (Amount)23, (Amount)0 };
        private static readonly Amount?[] NullableAmounts = new Amount?[] { (Amount)1, (Amount)23, (Amount)0, null };

        private static readonly Money[] MixedMoneys = new[] { 1 + Currency.EUR, 23 + Currency.USD };
        private static readonly Money?[] MixedNullableMoneys = new Money?[] { 1 + Currency.EUR, 23 + Currency.USD };

        private static readonly Money[] Moneys = new[] { 1 + Currency.EUR, 23 + Currency.EUR, 0 + Currency.EUR };
        private static readonly Money?[] NullableMoneys = new Money?[] { 1 + Currency.EUR, 23 + Currency.EUR, 0 + Currency.EUR, null };

        [Test]
        public void Reference_EnsuresThatSystemLinqDoesNotConflict()
        {
            var avg = new[] { 2.0, 3.0 }.Average();
            var sum = new[] { 2m, 3m }.Sum();

            Assert.AreEqual(2.5, avg);
            Assert.AreEqual(5m, sum);
        }

        [Test]
        public void Average_SelectedEmpyAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Amount>().Average((m) => m));
        }
        [Test]
        public void Average_EmptyNullableAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Amount>().Average());
        }
        [Test]
        public void Average_SelectedEmpyNullableAmount_Null()
        {
            Assert.IsNull(Array.Empty<Amount?>().Average((m) => m));
        }
        [Test]
        public void Average_EmptyNullableAmount_Null()
        {
            Assert.IsNull(Array.Empty<Amount?>().Average());
        }

        [Test]
        public void Average_SelectedAmount_Calculated()
        {
            var avg = Amounts.Average((m) => m);
            Assert.AreEqual((Amount)8, avg);
        }
        [Test]
        public void Average_Amount_Calculated()
        {
            var avg = Amounts.Average();
            Assert.AreEqual((Amount)8, avg);
        }
        [Test]
        public void Average_SelectedNullableAmount_Calculated()
        {
            var avg = NullableAmounts.Average((m) => m);
            Assert.AreEqual((Amount)8, avg);
        }
        [Test]
        public void Average_NullableAmount_Calculated()
        {
            var avg = NullableAmounts.Average();
            Assert.AreEqual((Amount)8, avg);
        }


        [Test]
        public void Average_SelectedEmpyMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Money>().Average((m) => m));
        }
        [Test]
        public void Average_EmptyNullableMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Money>().Average());
        }
        [Test]
        public void Average_SelectedEmpyNullableMoney_Null()
        {
            Assert.IsNull(Array.Empty<Money?>().Average((m) => m));
        }
        [Test]
        public void Average_EmptyNullableMoney_Null()
        {
            Assert.IsNull(Array.Empty<Money?>().Average());
        }

        [Test]
        public void Average_SelectedMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedMoneys.Average((m) => m));
        }
        [Test]
        public void Average_MixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedMoneys.Average());
        }
        [Test]
        public void Average_SelectedMixedNullableMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedNullableMoneys.Average((m) => m));
        }
        [Test]
        public void Average_NullableMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedNullableMoneys.Average());
        }

        [Test]
        public void Average_SelectedMoney_Calculated()
        {
            var avg = Moneys.Average((m) => m);
            Assert.AreEqual(8 + Currency.EUR, avg);
        }
        [Test]
        public void Average_Money_Calculated()
        {
            var avg = Moneys.Average();
            Assert.AreEqual(8 + Currency.EUR, avg);
        }
        [Test]
        public void Average_SelectedNullableMoney_Calculated()
        {
            var avg = NullableMoneys.Average((m) => m);
            Assert.AreEqual(8 + Currency.EUR, avg);
        }
        [Test]
        public void Average_NullableMoney_Calculated()
        {
            var avg = NullableMoneys.Average();
            Assert.AreEqual(8 + Currency.EUR, avg);
        }



        [Test]
        public void Sum_SelectedEmpyAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Amount>().Sum((m) => m));
        }
        [Test]
        public void Sum_EmptyNullableAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Amount>().Sum());
        }
        [Test]
        public void Sum_SelectedEmpyNullableAmount_Null()
        {
            Assert.IsNull(Array.Empty<Amount?>().Sum((m) => m));
        }
        [Test]
        public void Sum_EmptyNullableAmount_Null()
        {
            Assert.IsNull(Array.Empty<Amount?>().Sum());
        }

        [Test]
        public void Sum_SelectedAmount_Calculated()
        {
            var sum = Amounts.Sum((m) => m);
            Assert.AreEqual((Amount)24, sum);
        }
        [Test]
        public void Sum_Amount_Calculated()
        {
            var sum = Amounts.Sum();
            Assert.AreEqual((Amount)24, sum);
        }
        [Test]
        public void Sum_SelectedNullableAmount_Calculated()
        {
            var sum = NullableAmounts.Sum((m) => m);
            Assert.AreEqual((Amount)24, sum);
        }
        [Test]
        public void Sum_NullableAmount_Calculated()
        {
            var sum = NullableAmounts.Sum();
            Assert.AreEqual((Amount)24, sum);
        }


        [Test]
        public void Sum_SelectedEmpyMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Money>().Sum((m) => m));
        }
        [Test]
        public void Sum_EmptyNullableMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Array.Empty<Money>().Sum());
        }
        [Test]
        public void Sum_SelectedEmpyNullableMoney_Null()
        {
            Assert.IsNull(Array.Empty<Money?>().Sum((m) => m));
        }
        [Test]
        public void Sum_EmptyNullableMoney_Null()
        {
            Assert.IsNull(Array.Empty<Money?>().Sum());
        }

        [Test]
        public void Sum_SelectedMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedMoneys.Sum((m) => m));
        }
        [Test]
        public void Sum_MixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedMoneys.Sum());
        }
        [Test]
        public void Sum_SelectedMixedNullableMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedNullableMoneys.Sum((m) => m));
        }
        [Test]
        public void Sum_NullableMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => MixedNullableMoneys.Sum());
        }

        [Test]
        public void Sum_SelectedMoney_Calculated()
        {
            var sum = Moneys.Sum((m) => m);
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
        [Test]
        public void Sum_Money_Calculated()
        {
            var sum = Moneys.Sum();
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
        [Test]
        public void Sum_SelectedNullableMoney_Calculated()
        {
            var sum = NullableMoneys.Sum((m) => m);
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
        [Test]
        public void Sum_NullableMoney_Calculated()
        {
            var sum = NullableMoneys.Sum();
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
    }
}
