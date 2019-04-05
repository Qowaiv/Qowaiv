namespace Qowaiv.ComponentModel.DataAnnotations
{

    /// <summary>An object wrapper that can implicitly cast to a <see cref="bool"/>.</summary>
    /// <remarks>
    /// This helps <see cref="FluentValidationExtensions"/> to have fluent validation on objects.
    /// 
    /// Example:
    /// <code>
    /// if(obj.Mandatory().IsValid(new MinLengthAttribute(4))
    /// {
    ///     // do something.
    /// }
    /// </code>
    /// </remarks>
    public struct FluentValidationResult
    {
        /// <summary>represents a <see cref="FluentValidationResult"/> that wraps <code>false</code>.</summary>
        public static readonly FluentValidationResult False = new FluentValidationResult(false);

        /// <summary>Creates a new instance of a <see cref="FluentValidationResult"/>.</summary>
        /// <param name="value"></param>
        public FluentValidationResult(object value) => Value = value;

        /// <summary>The wrapped value.</summary>
        public object Value { get; }

        /// <summary>Implicitly cast the wrapper to a <see cref="bool"/>.</summary>
        /// <remarks>
        /// </remarks>
        public static implicit operator bool(FluentValidationResult result) => !(result.Value is bool bolean) || bolean;

        /// <summary>Determines if the specified <see cref="object"/> is equal to the underlying value.</summary>
        /// <param name="obj">
        /// The object to compare with.
        /// </param>
        public override bool Equals(object obj)
        {
            return Value is null ? obj is null : Value.Equals(obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Value is null ? 0 : Value.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => Value?.ToString();
    }
}
