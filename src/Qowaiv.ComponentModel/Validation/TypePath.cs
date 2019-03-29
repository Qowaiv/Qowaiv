using System;
using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a type path through representing the property chain.</summary>
    internal class TypePath : List<Type>
    {
        public TypePath() { }
        private TypePath(IEnumerable<Type> types) : base(types) { }

        /// <summary>Gets an extended <see cref="TypePath"/>.</summary>
        public TypePath GetExtended(Type type)
        {
            return new TypePath(this)
            {
                type
            };
        }
    }
}
