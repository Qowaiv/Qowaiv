using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.Web.Mvc;
using System;
using System.Linq;

namespace Qowaiv.UnitTests.Web.Mvc
{
    [TestClass]
    public class TypeConverterModelBinderTest
    {
        [TestMethod]
        public void Initialize_None_CountEquals()
        {
            var act = TypeConverterModelBinder.Types.ToArray();

            Assert.AreEqual(9, act.Length);
        }
        
        [TestMethod]
        public void AddType_Int32_OneItemAdded()
        {
            try
            {
                var exp = TypeConverterModelBinder.Types.Count() + 1;
                TypeConverterModelBinder.AddType(typeof(Int32));
                var act = TypeConverterModelBinder.Types.Count();
                
                Assert.AreEqual(exp, act);
            }
            finally
            {
                // Cleanup
                TypeConverterModelBinder.RemoveType(typeof(Int32));
            }
        }

        [TestMethod]
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