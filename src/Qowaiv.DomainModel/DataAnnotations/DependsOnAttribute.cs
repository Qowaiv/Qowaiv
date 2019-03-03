using System;

namespace Qowaiv.DomainModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DependsOnAttribute: Attribute
    {
        public string[] DependingProperties { get; }

        private DependsOnAttribute() { }

        public DependsOnAttribute(string other)
            : this(new string[] { other }) { }

        public DependsOnAttribute(params string[] others)
        {
            DependingProperties = others;
        }
    }
}
