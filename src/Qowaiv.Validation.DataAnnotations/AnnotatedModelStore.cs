using Qowaiv.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.Validation.DataAnnotations
{
    /// <summary>Represents a store for <see cref="AnnotatedModel"/>s.</summary>
    public sealed class AnnotatedModelStore
    {
        /// <summary>Creates a new instance of an <see cref="AnnotatedModelStore"/>.</summary>
        internal AnnotatedModelStore()
        {
            _models = new ConcurrentDictionary<Type, AnnotatedModel>(new Dictionary<Type, AnnotatedModel>
            {
                { typeof(Guid), AnnotatedModel.None },
                { typeof(DateTime), AnnotatedModel.None },
                { typeof(DateTimeOffset), AnnotatedModel.None },
            });
            foreach (var tp in typeof(Date).Assembly.GetTypes().Where(tp => tp.IsValueType && tp.IsVisible))
            {
                _models[tp] = AnnotatedModel.None;
            }
        }

        /// <summary>Gets the stored <see cref="AnnotatedModel"/>s.</summary>
        private readonly ConcurrentDictionary<Type, AnnotatedModel> _models;

        /// <summary>Gets an <see cref="AnnotatedModel"/> based on the <see cref="Type"/>.</summary>
        public AnnotatedModel GetAnnotededModel(Type type)
        {
            Guard.NotNull(type, nameof(type));

            var tp = QowaivType.GetNotNullableType(type);

            if (tp.IsEnum || type.IsPrimitive)
            {
                return AnnotatedModel.None;
            }
            if (!_models.TryGetValue(tp, out AnnotatedModel model))
            {
                model = AnnotatedModel.Create(tp);
                _models[type] = model;
            }
            return model;
        }
    }
}
