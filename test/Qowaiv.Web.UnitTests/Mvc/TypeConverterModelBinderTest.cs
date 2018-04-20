using NUnit.Framework;
using Qowaiv.Web.Mvc;
using System.Linq;

namespace Qowaiv.Web.UnitTests.Mvc
{
    public class TypeConverterModelBinderTest
    {
        [Test]
        public void Initialize_None_CountEquals()
        {
            var act = TypeConverterModelBinder.Types.ToArray();

            Assert.AreEqual(21, act.Length);
        }
        
        [Test]
        public void AddType_Int32_OneItemAdded()
        {
            try
            {
                var exp = TypeConverterModelBinder.Types.Count() + 1;
                TypeConverterModelBinder.AddType(typeof(int));
                var act = TypeConverterModelBinder.Types.Count();
                
                Assert.AreEqual(exp, act);
            }
            finally
            {
                // Cleanup
                TypeConverterModelBinder.RemoveType(typeof(int));
            }
        }

        [Test]
        public void AddType_ClassWithoutConverter_NothingAdded()
        {
            var exp = TypeConverterModelBinder.Types.Count();
            TypeConverterModelBinder.AddType(typeof(ClassWithoutConverter));
            var act = TypeConverterModelBinder.Types.Count();

            Assert.AreEqual(exp, act);
        }
    }

    public class ClassWithoutConverter { }

}
