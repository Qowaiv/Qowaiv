using System;

namespace Qowaiv.Validation.DataAnnotations
{
    /// <summary>Decorates a class so that the <see cref="AnnotatedModelValidator"/>
    /// will also validate its children.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NestedModelAttribute : Attribute { }
}
