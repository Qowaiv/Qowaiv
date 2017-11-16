using System;
using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a store for <see cref="AnnotatedModel"/>s.</summary>
    public sealed class AnnotatedModelStore
    {
        /// <summary>Gets the singleton instance of the <see cref="AnnotatedModelStore"/>.</summary>
        public static readonly AnnotatedModelStore Instance = new AnnotatedModelStore();

        /// <summary>Creates a new instance of an <see cref="AnnotatedModelStore"/>.</summary>
        private AnnotatedModelStore()
        {
            Models = new Dictionary<Type, AnnotatedModel>();
            NotAnnotatedTypes = new HashSet<Type>();
        }

        /// <summary>Gets the stored <see cref="AnnotatedModel"/>s.</summary>
        private IDictionary<Type, AnnotatedModel> Models { get; }

        /// <summary>Gets the stored <see cref="Type"/>s that are not annotated.</summary>
        private ISet<Type> NotAnnotatedTypes { get; }

        /// <summary>Gets an <see cref="AnnotatedModel"/> based on the <see cref="Type"/>.</summary>
        public AnnotatedModel GetAnnotededModel(Type type)
        {
            Guard.NotNull(type, nameof(type));

            lock (locker)
            {
                if (NotAnnotatedTypes.Contains(type))
                {
                    return AnnotatedModel.None(type);
                }
                if (!Models.TryGetValue(type, out AnnotatedModel model))
                {
                    model = AnnotatedModel.Create(type);

                    // store the result.
                    if (model.IsValidatable)
                    {
                        Models[type] = model;
                    }
                    else
                    {
                        NotAnnotatedTypes.Add(type);
                    }
                }
                return model;

            }
        }

        /// <summary>To be thread-safe we have a locker.</summary>
        private readonly object locker = new object();
    }
}
