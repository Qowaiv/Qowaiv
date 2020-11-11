using Qowaiv;
using Qowaiv.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVO_specs
{
    public class All
    {
        internal static readonly Type[] NoneSvos = new[] { typeof(FormattingArguments) };

        public static IEnumerable<Type> Types
        {
            get => typeof(SingleValueObjectAttribute).Assembly
                .GetTypes()
                .Where(tp => tp.IsValueType && tp.IsPublic && !tp.IsEnum && !tp.IsAbstract)
                .Except(NoneSvos);
        }
    }
}
