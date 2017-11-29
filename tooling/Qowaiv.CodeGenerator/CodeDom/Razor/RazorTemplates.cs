using Qowaiv.CodeGenerator.Generators;
using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    public static class RazorTemplates
    {
        public static RazorTemplate<T> Select<T>(string postfix = null)
        {
            var className = $"{typeof(T).Name}{postfix}";

            var path = GetResources().FirstOrDefault(name => GetNoDots(name) == className);

            if (path == null)
            {
                throw new TemplateNotFoundException(typeof(T), postfix);
            }
            var stream = typeof(SvoStructGenerator).Assembly.GetManifestResourceStream(path);
            return new RazorTemplate<T>(stream, className);
        }

        private static string GetNoDots(string name)
        {
            var index = name.IndexOf('1');
            var nodots = name.Substring(index + 1, name.LastIndexOf('.'));

            while (nodots.Contains('.'))
            {
                nodots = nodots.Substring(nodots.IndexOf('.') + 1);
            }
            return nodots;
        }

        private static IEnumerable<string> GetResources()
        {
            return typeof(SvoStructGenerator).Assembly
                .GetManifestResourceNames()
                .Where(name => name.EndsWith(".cshtml"));
        }
    }
}
