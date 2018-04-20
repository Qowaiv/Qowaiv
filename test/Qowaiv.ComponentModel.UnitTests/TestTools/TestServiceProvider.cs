using System;
using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Tests.TestTools
{
    public class TestServiceProvider : Dictionary<Type, object>, IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return this[serviceType];
        }
    }
}
