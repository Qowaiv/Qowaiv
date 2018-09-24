using Qowaiv;
using Qowaiv.ComponentModel.DataAnnotations;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>Extensions on <see cref="ValidationContext"/>.</summary>
    public static class ValidationContextExtensions
    {
        /// <summary>Returns the service that provides custom validation.</summary>
        public static TService GetSevice<TService>(this ValidationContext validationContext)
        {
            Guard.NotNull(validationContext, nameof(validationContext));
            return (TService)validationContext.GetService(typeof(TService));
        }

        /// <summary>Casts a validation context to a typed validation context.</summary>
        public static ValidationContext<TModel> Typed<TModel>(this ValidationContext validationContext)
            where TModel: class
        {
            if (validationContext is null)
            {
                return null;
            }
            Guard.IsTypeOf<TModel>(validationContext.ObjectInstance, nameof(validationContext.ObjectInstance));

            return new ValidationContext<TModel>(validationContext);
        }
    }
}
