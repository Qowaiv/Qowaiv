using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
    /// <summary>Describes the context in which a validation check is performed.</summary>
    public sealed class ValidationContext<TModel>
        where TModel : class
    {
        private readonly ValidationContext _context;

        /// <summary>Creates a new instance of a typed <see cref="ValidationContext"/>.</summary>
        internal ValidationContext(ValidationContext context) => _context = context;

        /// <summary>Gets the name of the member to validate.</summary>
        public string DisplayName => _context.DisplayName;

        /// summary>Gets the dictionary of key/value pairs that is associated with this context.</summary>
        public IDictionary<object, object> Items => _context.Items;

        /// <summary>Gets  the name of the member to validate.</summary>
        public string MemberName => _context.MemberName;

        /// <summary>Gets the model to validate.</summary>
        public TModel Model => (TModel)_context.ObjectInstance;

        /// <summary>Returns the service that provides custom validation.</summary>
        public TService GetService<TService>() => _context.GetSevice<TService>();

        /// <summary>Returns the service that provides custom validation.</summary>
        public object GetService(Type serviceType) => _context.GetService(serviceType);

        /// <summary>Casts a typed validation context to a not typed validation context.</summary>
        public static implicit operator ValidationContext(ValidationContext<TModel> typed) => typed?._context;
    }
}
