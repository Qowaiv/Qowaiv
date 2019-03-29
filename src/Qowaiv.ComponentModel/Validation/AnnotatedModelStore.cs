using Qowaiv.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a store for <see cref="AnnotatedModel"/>s.</summary>
    public sealed class AnnotatedModelStore
    {
        /// <summary>Creates a new instance of an <see cref="AnnotatedModelStore"/>.</summary>
        internal AnnotatedModelStore()
        {
            Models = new Dictionary<Type, AnnotatedModel>
            {
                { typeof(string), AnnotatedModel.None },
                { typeof(char), AnnotatedModel.None },
                { typeof(short), AnnotatedModel.None },
                { typeof(int), AnnotatedModel.None },
                { typeof(long), AnnotatedModel.None },
                { typeof(ushort), AnnotatedModel.None },
                { typeof(uint), AnnotatedModel.None },
                { typeof(ulong), AnnotatedModel.None },
                { typeof(float), AnnotatedModel.None },
                { typeof(double), AnnotatedModel.None },
                { typeof(decimal), AnnotatedModel.None },
                { typeof(Guid), AnnotatedModel.None },
                { typeof(DateTime), AnnotatedModel.None },
                { typeof(DateTimeOffset), AnnotatedModel.None },
            };
            foreach(var tp in typeof(Uuid).Assembly.GetTypes().Where(tp => tp.IsValueType && tp.IsVisible))
            {
                Models[tp] = AnnotatedModel.None;
            }
        }

        /// <summary>Gets the stored <see cref="AnnotatedModel"/>s.</summary>
        private IDictionary<Type, AnnotatedModel> Models { get; }

        /// <summary>Gets an <see cref="AnnotatedModel"/> based on the <see cref="Type"/>.</summary>
        public AnnotatedModel GetAnnotededModel(Type type)
        {
            Guard.NotNull(type, nameof(type));

            var tp = QowaivType.GetNotNullableType(type);

            if (Models.TryGetValue(tp, out AnnotatedModel model))
            {
                return model;
            }
            if (tp.IsEnum)
            {
                return AnnotatedModel.None;
            }
            lock (locker)
            {
                if (!Models.TryGetValue(tp, out model))
                {
                    model = AnnotatedModel.Create(tp);
                    Models[type] = model;
                }
                return model;
            }
        }

        ///// <summary>To be thread-safe we have a locker.</summary>
        private readonly object locker = new object();
    }
}
