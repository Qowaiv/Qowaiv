using NUnit.Framework;
using System.Globalization;

namespace Qowaiv.UnitTests
{
    public class PercentageTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Percentage TestStruct = 0.1751m;

        public static NumberFormatInfo GetCustomNumberFormatInfo()
        {
            var info = new NumberFormatInfo
            {
                PercentSymbol = "!",
                PerMilleSymbol = "#",
                PercentDecimalSeparator = "*",
            };
            return info;
        }


        #region Percentage manipulation tests

        [TestCase(0.1234, -0.1234)]
        [TestCase(0.1234, +0.1234)]
        public void Abs(Percentage expected, Percentage value)
        {
            var abs = value.Abs();
            Assert.AreEqual(expected, abs);
        }

        [Test]
        public void UnaryNegation_Percentage17_Min17()
        {
            Percentage act = 0.17m;
            Percentage exp = -0.17m;

            act = -act;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void UnaryPlus_Percentage17_17()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.17m;

            act = +act;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Devide_Percentage17Percentage50_34()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.34m;
            Percentage mut = 0.50m;

            act /= mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Add_Percentage17Percentage42_59()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.59m;
            Percentage mut = 0.42m;

            act += mut;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtract_Percentage17Percentage13_59()
        {
            Percentage act = 0.17m;
            Percentage exp = 0.04m;
            Percentage mut = 0.13m;

            act -= mut;

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Number manipulation tests

        [Test]
        public void Increment_Percentage10_Percentage11()
        {
            Percentage act = 0.1m;
            Percentage exp = 0.11m;
            act++;

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Decrement_Percentage10_Percentage09()
        {
            Percentage act = 0.1m;
            Percentage exp = 0.09m;
            act--;

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Subtraction_Int1617Percentage10_16()
        {
            short act = 17;
            short exp = 16;
            act -= 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Int3217Percentage10_16()
        {
            int act = 17;
            int exp = 16;
            act -= 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Int6417Percentage10_16()
        {
            long act = 17;
            long exp = 16;
            act -= 10.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_UInt1617Percentage50_9()
        {
            ushort act = 17;
            ushort exp = 9;
            act -= 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_UInt3217Percentage50_9()
        {
            uint act = 17;
            uint exp = 9;
            act -= 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_UInt6417Percentage50_9()
        {
            ulong act = 17;
            ulong exp = 9;
            act -= 50.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Decimal17Percentage17_11D39()
        {
            decimal act = 17;
            decimal exp = 11.39m;
            act -= 33.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Double17Percentage17_11D39()
        {
            double act = 17;
            double exp = 11.39;
            act -= 33.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Subtraction_Single17Percentage17_11D3899994()
        {
            float act = 17;
            float exp = 11.3899994F;
            act -= 33.Percent();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Division_Int1617Percentage51_33()
        {
            short act = 17;
            short exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Int3217Percentage51_33()
        {
            int act = 17;
            int exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Int6417Percentage51_33()
        {
            long act = 17;
            long exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_UInt1617Percentage51_33()
        {
            ushort act = 17;
            ushort exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_UInt3217Percentage51_33()
        {
            uint act = 17;
            uint exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_UInt6417Percentage51_33()
        {
            ulong act = 17;
            ulong exp = 33;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Decimal17Percentage51_33()
        {
            decimal act = 17;
            decimal exp = 100.0m / 3.0m;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Double17Percentage51_33()
        {
            double act = 17;
            double exp = 100.0 / 3.0;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Division_Single17Percentage51_33()
        {
            float act = 17;
            float exp = 100.0F / 3.0F;
            act /= 51.Percent();

            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Multiply_Int1617Percentage51_8()
        {
            short act = 17;
            short exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Int3217Percentage51_8()
        {
            int act = 17;
            int exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Int6417Percentage51_8()
        {
            long act = 17;
            long exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_UInt1617Percentage51_8()
        {
            ushort act = 17;
            ushort exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_UInt3217Percentage51_8()
        {
            uint act = 17;
            uint exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_UInt6417Percentage51_8()
        {
            ulong act = 17;
            ulong exp = 8;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Decimal17Percentage51_8D67()
        {
            decimal act = 17;
            decimal exp = 8.67m;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Double17Percentage51_8D67()
        {
            double act = 17;
            double exp = 8.67;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }
        [Test]
        public void Multiply_Single17Percentage51_8D67()
        {
            float act = 17;
            float exp = 8.67F;
            act *= 51.Percent();

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region Math-like methods tests

        [Test]
        public void Max_FirstLargest_First()
        {
            var actual = Percentage.Max(TestStruct, 0.007);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Max_SecondLargest_Second()
        {
            var actual = Percentage.Max(0.1, TestStruct);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Max_Values_Largest()
        {
            var actual = Percentage.Max(0.1, TestStruct, 0.02, 0.17);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Min_FirstSmallest_Frist()
        {
            var actual = Percentage.Min(TestStruct, 0.9);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Min_SecondSmallest_Second()
        {
            var actual = Percentage.Min(0.9, TestStruct);
            Assert.AreEqual(TestStruct, actual);
        }

        [Test]
        public void Min_Values_Smallest()
        {
            var actual = Percentage.Min(0.9, TestStruct, 0.18, 0.74);
            Assert.AreEqual(TestStruct, actual);
        }

       


        #endregion
    }
}
