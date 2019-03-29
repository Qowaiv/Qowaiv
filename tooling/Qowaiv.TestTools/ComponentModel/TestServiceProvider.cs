using System;
using System.Collections.Generic;

namespace Qowaiv.TestTools.ComponentModel
{
    /// <summary>Extreme simple <see cref="IServiceProvider"/>.</summary>
    public class TestServiceProvider : Dictionary<Type, object>, IServiceProvider
    {
        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            return this[serviceType];
        }
    }
}
