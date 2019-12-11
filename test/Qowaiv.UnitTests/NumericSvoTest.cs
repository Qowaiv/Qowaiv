using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Qowaiv.UnitTests
{
    public class NumericSvoTest
    {
        internal const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        internal const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
        internal const BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static;


        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        [TestCase(typeof(StreamSize))]
        public void Abs(Type svo)
        {
            //Math.Abs
            var methods = svo?
                .GetMethods(PublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Abs))
                .SelectParameters()
                .Where(pars => pars.Length == 0)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        [TestCase(typeof(StreamSize))]
        public void Plus(Type svo)
        {
            var methods = svo
                .GetMethods(NonPublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Plus))
                .SelectParameters()
                .Where(pars => pars.Length == 0)
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator("UnaryPlus")
                .SelectParameters()
                .Where(pars => pars.Length == 1 && pars[0] == svo)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
            Assert.IsTrue(operators.Length == 1, nameof(operators));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        [TestCase(typeof(StreamSize))]
        public void Negate(Type svo)
        {
            var methods = svo
                .GetMethods(NonPublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Negate))
                .SelectParameters()
                .Where(pars => pars.Length == 0)
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator("UnaryNegation")
                .SelectParameters()
                .Where(pars => pars.Length == 1 && pars[0] == svo)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
            Assert.IsTrue(operators.Length == 1, nameof(operators));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        [TestCase(typeof(StreamSize))]
        public void Increment(Type svo)
        {
            var methods = svo
                .GetMethods(NonPublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Increment))
                .SelectParameters()
                .Where(pars => pars.Length == 0)
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator(nameof(Increment))
                .SelectParameters()
                .Where(pars => pars.Length == 1 && pars[0] == svo)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
            Assert.IsTrue(operators.Length == 1, nameof(operators));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        [TestCase(typeof(StreamSize))]
        public void Decrement(Type svo)
        {
            var methods = svo
                .GetMethods(NonPublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Decrement))
                .SelectParameters()
                .Where(pars => pars.Length == 0)
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator(nameof(Decrement))
                .SelectParameters()
                .Where(pars => pars.Length == 1 && pars[0] == svo)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
            Assert.IsTrue(operators.Length == 1, nameof(operators));
        }

        [TestCase(typeof(Amount), typeof(Amount), typeof(Percentage))]
        [TestCase(typeof(Money), typeof(Money), typeof(Percentage))]
        [TestCase(typeof(Percentage), typeof(Percentage))]
        [TestCase(typeof(StreamSize), typeof(StreamSize), typeof(Percentage))]
        public void Add(Type svo, params Type[] expected)
        {
            var methods = svo
                .GetMethods(PublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Add))
                .SelectParameters()
                .Where(pars => pars.Length == 1)
                .Select(pars => pars[0])
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator("Addition")
                .SelectParameters()
                .Where(pars => pars.Length == 2 && pars[0] == svo)
                .Select(pars => pars[1])
                .ToArray();

            CollectionAssert.AreEquivalent(expected, methods, nameof(methods));
            CollectionAssert.AreEquivalent(expected, operators, nameof(operators));
        }

        [TestCase(typeof(Amount), typeof(Amount), typeof(Percentage))]
        [TestCase(typeof(Money), typeof(Money), typeof(Percentage))]
        [TestCase(typeof(Percentage), typeof(Percentage))]
        [TestCase(typeof(StreamSize), typeof(StreamSize), typeof(Percentage))]
        public void Subtract(Type svo, params Type[] expected)
        {
            var methods = svo
                .GetMethods(PublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Subtract))
                .SelectParameters()
                .Where(pars => pars.Length == 1)
                .Select(pars => pars[0])
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator("Subtraction")
                .SelectParameters()
                .Where(pars => pars.Length == 2 && pars[0] == svo)
                .Select(pars => pars[1])
                .ToArray();

            CollectionAssert.AreEquivalent(expected, methods, nameof(methods));
            CollectionAssert.AreEquivalent(expected, operators, nameof(operators));
        }

        [TestCase(typeof(Amount), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        [TestCase(typeof(Money), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        [TestCase(typeof(Percentage), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        [TestCase(typeof(StreamSize), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        public void Multiply(Type svo, params Type[] expected)
        {
            var methods = svo
                .GetMethods(PublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Multiply))
                .SelectParameters()
                .Where(pars => pars.Length == 1)
                .Select(pars => pars[0])
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator(nameof(Multiply))
                .SelectParameters()
                .Where(pars => pars.Length == 2 && pars[0] == svo)
                .Select(pars => pars[1])
                .ToArray();

            CollectionAssert.AreEquivalent(expected, methods, nameof(methods));
            CollectionAssert.AreEquivalent(expected, operators, nameof(operators));
        }

        [TestCase(typeof(Amount), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        [TestCase(typeof(Money), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        [TestCase(typeof(Percentage), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        [TestCase(typeof(StreamSize), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(Percentage))]
        public void Divide(Type svo, params Type[] expected)
        {
            var methods = svo
                .GetMethods(PublicInstance)
                .ExcludeObsolete()
                .Returns(svo)
                .Where(m => m.Name == nameof(Divide))
                .SelectParameters()
                .Where(pars => pars.Length == 1)
                .Select(pars => pars[0])
                .ToArray();

            var operators = svo
                .GetMethods(PublicStatic)
                .ExcludeObsolete()
                .Returns(svo)
                .IsOperator("Division")
                .SelectParameters()
                .Where(pars => pars.Length == 2 && pars[0] == svo)
                .Select(pars => pars[1])
                .ToArray();

            CollectionAssert.AreEquivalent(expected, methods, nameof(methods));
            CollectionAssert.AreEquivalent(expected, operators, nameof(operators));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        public void Round(Type svo)
        {
            var methods = svo?
                .GetMethods(PublicInstance)
                .Returns(svo)
                .Where(m => m.Name == nameof(Round))
                .SelectParameters()
                .Where(pars => pars.Length == 0)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        public void Round_Decimals(Type svo)
        {
            var methods = svo?
                .GetMethods(PublicInstance)
                .Returns(svo)
                .Where(m => m.Name == nameof(Round))
                .SelectParameters()
                .Where(pars => pars.Length == 1 && pars[0] == typeof(int))
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
        }

        [TestCase(typeof(Amount))]
        [TestCase(typeof(Money))]
        [TestCase(typeof(Percentage))]
        public void Round_Decimals_DecimalRounding(Type svo)
        {
            var methods = svo?
                .GetMethods(PublicInstance)
                .Returns(svo)
                .Where(m => m.Name == nameof(Round))
                .SelectParameters()
                .Where(pars => pars.Length == 2 && pars[0] == typeof(int) && pars[1] == typeof(DecimalRounding))
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
        }

        [TestCase(typeof(Amount), typeof(decimal))]
        [TestCase(typeof(Money), typeof(decimal))]
        [TestCase(typeof(Percentage), typeof(Percentage))]
        public void RoundToMultiple(Type svo, Type multiplyer)
        {
            var methods = svo?
                .GetMethods(PublicInstance)
                .Returns(svo)
                .Where(m => m.Name == nameof(RoundToMultiple))
                .SelectParameters()
                .Where(pars => pars.Length == 1 && pars[0] == multiplyer)
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
        }

        [TestCase(typeof(Amount), typeof(decimal))]
        [TestCase(typeof(Money), typeof(decimal))]
        [TestCase(typeof(Percentage), typeof(Percentage))]
        public void RoundToMultiple_DecimalRounding(Type svo, Type multiplyer)
        {
            var methods = svo?
                .GetMethods(PublicInstance)
                .Returns(svo)
                .Where(m => m.Name == nameof(RoundToMultiple))
                .SelectParameters()
                .Where(pars => pars.Length == 2 && pars[0] == multiplyer && pars[1] == typeof(DecimalRounding))
                .ToArray();

            Assert.IsTrue(methods.Length == 1, nameof(methods));
        }

    }

    internal static class NumericSvoTestExtensions
    {
        public static IEnumerable<MethodInfo> ExcludeObsolete(this IEnumerable<MethodInfo> infos)
        {
            foreach (var info in infos)
            {
                if (info.GetCustomAttribute<ObsoleteAttribute>() is null)
                {
                    yield return info;
                }
            }
        }

        public static IEnumerable<MethodInfo> Returns(this IEnumerable<MethodInfo> infos, Type returnType)
        {
            return infos.Where(info => info.ReturnType == returnType);
        }

        public static IEnumerable<MethodInfo> IsOperator(this IEnumerable<MethodInfo> infos, string nameSuffix)
        {
            return infos.Where(info => info.IsSpecialName && info.Name == "op_" + nameSuffix);
        }

        public static IEnumerable<Type[]> SelectParameters(this IEnumerable<MethodInfo> infos)
        {
            return infos.Select(info => info.GetParameters().Select(par => par.ParameterType).ToArray());
        }
    }
}
