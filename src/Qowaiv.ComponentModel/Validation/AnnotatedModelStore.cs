using System;
using System.Collections.Generic;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a store for <see cref="AnnotatedModel"/>s.</summary>
    public sealed class AnnotatedModelStore
    {
        /// <summary>Creates a new instance of an <see cref="AnnotatedModelStore"/>.</summary>
        internal AnnotatedModelStore()
        {
            Models = new Dictionary<Type, AnnotatedModel>();
            NotAnnotatedTypes = new HashSet<Type>
            {
                typeof(string),
                typeof(int),
                typeof(long),
                typeof(double),
                typeof(decimal),
                typeof(Guid),
            };
        }

        /// <summary>Gets the stored <see cref="AnnotatedModel"/>s.</summary>
        private IDictionary<Type, AnnotatedModel> Models { get; }

        /// <summary>Gets the stored <see cref="Type"/>s that are not annotated.</summary>
        private ISet<Type> NotAnnotatedTypes { get; }

        /// <summary>Gets an <see cref="AnnotatedModel"/> based on the <see cref="Type"/>.</summary>
        public AnnotatedModel GetAnnotededModel(Type type)
        {
            if (NotAnnotatedTypes.Contains(type))
            {
                return AnnotatedModel.None(type);
            }
            if (Models.TryGetValue(type, out AnnotatedModel model))
            {
                return model;
            }
            lock (locker)
            {
                return GetAnnotededModel(type, new TypePath());
            }
        }

        private AnnotatedModel GetAnnotededModel(Type type, TypePath path)
        {
            if (NotAnnotatedTypes.Contains(type))
            {
                return AnnotatedModel.None(type);
            }
            if (!Models.TryGetValue(type, out AnnotatedModel model))
            {
                var extended = path.GetExtended(type);
                model = AnnotatedModel.Create(type, this, extended);

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

        internal bool IsAnnotededModel(Type type, TypePath path)
        {
            Guard.NotNull(type, nameof(type));

            if (NotAnnotatedTypes.Contains(type))
            {
                return false;
            }
            if (Models.ContainsKey(type))
            {
                return true;
            }
            // We will not check the model if it was already added to the chain.
            if (!path.Contains(type) && GetAnnotededModel(type, path).IsValidatable)
            {
                return true;
            }
            else
            {
                NotAnnotatedTypes.Add(type);
                return false;
            }
        }

        /// <summary>To be thread-safe we have a locker.</summary>
        private readonly object locker = new object();
    }
}
