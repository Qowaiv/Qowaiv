using NUnit.Framework;
using Qowaiv.Financial;
using System;

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
        public void Average_SelectedEmpyAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Average(Array.Empty<Amount>(), (m) => m));
        }
        [Test]
        public void Average_EmptyNullableAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Average(Array.Empty<Amount>()));
        }
        [Test]
        public void Average_SelectedEmpyNullableAmount_Null()
        {
            Assert.IsNull(QowaivEnumerable.Average(Array.Empty<Amount?>(), (m) => m));
        }
        [Test]
        public void Average_EmptyNullableAmount_Null()
        {
            Assert.IsNull(QowaivEnumerable.Average(Array.Empty<Amount?>()));
        }

        [Test]
        public void Average_SelectedAmount_Calculated()
        {
            var avg = QowaivEnumerable.Average(Amounts, (m) => m);
            Assert.AreEqual((Amount)8, avg);
        }
        [Test]
        public void Average_Amount_Calculated()
        {
            var avg = QowaivEnumerable.Average(Amounts);
            Assert.AreEqual((Amount)8, avg);
        }
        [Test]
        public void Average_SelectedNullableAmount_Calculated()
        {
            var avg = QowaivEnumerable.Average(NullableAmounts, (m) => m);
            Assert.AreEqual((Amount)8, avg);
        }
        [Test]
        public void Average_NullableAmount_Calculated()
        {
            var avg = QowaivEnumerable.Average(NullableAmounts);
            Assert.AreEqual((Amount)8, avg);
        }


        [Test]
        public void Average_SelectedEmpyMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Average(Array.Empty<Money>(), (m) => m));
        }
        [Test]
        public void Average_EmptyNullableMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Average(Array.Empty<Money>()));
        }
        [Test]
        public void Average_SelectedEmpyNullableMoney_Null()
        {
            Assert.IsNull(QowaivEnumerable.Average(Array.Empty<Money?>(), (m) => m));
        }
        [Test]
        public void Average_EmptyNullableMoney_Null()
        {
            Assert.IsNull(QowaivEnumerable.Average(Array.Empty<Money?>()));
        }

        [Test]
        public void Average_SelectedMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Average(MixedMoneys, (m) => m));
        }
        [Test]
        public void Average_MixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Average(MixedMoneys));
        }
        [Test]
        public void Average_SelectedMixedNullableMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Average(MixedNullableMoneys, (m) => m));
        }
        [Test]
        public void Average_NullableMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Average(MixedNullableMoneys));
        }

        [Test]
        public void Average_SelectedMoney_Calculated()
        {
            var avg = QowaivEnumerable.Average(Moneys, (m) => m);
            Assert.AreEqual(8 + Currency.EUR, avg);
        }
        [Test]
        public void Average_Money_Calculated()
        {
            var avg = QowaivEnumerable.Average(Moneys);
            Assert.AreEqual(8 + Currency.EUR, avg);
        }
        [Test]
        public void Average_SelectedNullableMoney_Calculated()
        {
            var avg = QowaivEnumerable.Average(NullableMoneys, (m) => m);
            Assert.AreEqual(8 + Currency.EUR, avg);
        }
        [Test]
        public void Average_NullableMoney_Calculated()
        {
            var avg = QowaivEnumerable.Average(NullableMoneys);
            Assert.AreEqual(8 + Currency.EUR, avg);
        }



        [Test]
        public void Sum_SelectedEmpyAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Sum(Array.Empty<Amount>(), (m) => m));
        }
        [Test]
        public void Sum_EmptyNullableAmount_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Sum(Array.Empty<Amount>()));
        }
        [Test]
        public void Sum_SelectedEmpyNullableAmount_Null()
        {
            Assert.IsNull(QowaivEnumerable.Sum(Array.Empty<Amount?>(), (m) => m));
        }
        [Test]
        public void Sum_EmptyNullableAmount_Null()
        {
            Assert.IsNull(QowaivEnumerable.Sum(Array.Empty<Amount?>()));
        }

        [Test]
        public void Sum_SelectedAmount_Calculated()
        {
            var sum = QowaivEnumerable.Sum(Amounts, (m) => m);
            Assert.AreEqual((Amount)24, sum);
        }
        [Test]
        public void Sum_Amount_Calculated()
        {
            var sum = QowaivEnumerable.Sum(Amounts);
            Assert.AreEqual((Amount)24, sum);
        }
        [Test]
        public void Sum_SelectedNullableAmount_Calculated()
        {
            var sum = QowaivEnumerable.Sum(NullableAmounts, (m) => m);
            Assert.AreEqual((Amount)24, sum);
        }
        [Test]
        public void Sum_NullableAmount_Calculated()
        {
            var sum = QowaivEnumerable.Sum(NullableAmounts);
            Assert.AreEqual((Amount)24, sum);
        }


        [Test]
        public void Sum_SelectedEmpyMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Sum(Array.Empty<Money>(), (m) => m));
        }
        [Test]
        public void Sum_EmptyNullableMoney_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => QowaivEnumerable.Sum(Array.Empty<Money>()));
        }
        [Test]
        public void Sum_SelectedEmpyNullableMoney_Null()
        {
            Assert.IsNull(QowaivEnumerable.Sum(Array.Empty<Money?>(), (m) => m));
        }
        [Test]
        public void Sum_EmptyNullableMoney_Null()
        {
            Assert.IsNull(QowaivEnumerable.Sum(Array.Empty<Money?>()));
        }

        [Test]
        public void Sum_SelectedMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Sum(MixedMoneys, (m) => m));
        }
        [Test]
        public void Sum_MixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Sum(MixedMoneys));
        }
        [Test]
        public void Sum_SelectedMixedNullableMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Sum(MixedNullableMoneys, (m) => m));
        }
        [Test]
        public void Sum_NullableMixedMoney_Throws()
        {
            Assert.Throws<CurrencyMismatchException>(() => QowaivEnumerable.Sum(MixedNullableMoneys));
        }

        [Test]
        public void Sum_SelectedMoney_Calculated()
        {
            var sum = QowaivEnumerable.Sum(Moneys, (m) => m);
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
        [Test]
        public void Sum_Money_Calculated()
        {
            var sum = QowaivEnumerable.Sum(Moneys);
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
        [Test]
        public void Sum_SelectedNullableMoney_Calculated()
        {
            var sum = QowaivEnumerable.Sum(NullableMoneys, (m) => m);
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
        [Test]
        public void Sum_NullableMoney_Calculated()
        {
            var sum = QowaivEnumerable.Sum(NullableMoneys);
            Assert.AreEqual(24 + Currency.EUR, sum);
        }
    }
}
