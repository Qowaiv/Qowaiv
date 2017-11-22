namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    /// <summary>Represents a typed razor template.</summary>
    public abstract class RazorTemplate<T> : RazorTemplate
    {
        /// <summary>The underlying typed Model.</summary>
        public T Model { get; set; }
    }
}
