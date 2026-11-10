using System.IO;
using System.Text;

namespace System.Xml.Linq;

/// <summary>Extensions on <see cref="XObject"/>.</summary>
public static class QowaivXObjectExtensions
{
    extension(XDocument document)
    {
        /// <summary>Validates the document using a <see cref="Stream"/> representing an <see cref="XmlSchema"/>.</summary>
        [Pure]
        public Result<XDocument> Validate(Stream stream)
            => document.Validate(stream.AsXmlSchema());

        /// <summary>Validates the document using an <see cref="XmlSchema"/>.</summary>
        [Pure]
        public Result<XDocument> Validate(XmlSchema schema)
            => document.Validate(new XmlSchemaSet().Append(schema));

        /// <summary>Validates the document using an <see cref="XmlSchemaSet"/>.</summary>
        [Pure]
        public Result<XDocument> Validate(XmlSchemaSet schemas)
        {
            Guard.NotNull(document);
            Guard.NotNull(schemas);

            var handler = new XmlValidationHandler();
            document.Validate(schemas, handler.Validate);
            return Result.For(document, handler.Messages);
        }
    }

    extension(XAttribute attribute)
    {
        /// <summary>Get the absolute XPath of the attribute.</summary>
        /// <remarks>
        /// e.g.: /people/person[6]/name[1]/last[1]/@prefix.
        /// </remarks>
        [Pure]
        internal string AbsoluteXPath()
            => $"{attribute.Parent?.AbsoluteXPath()}/@{attribute.Name.LocalName}";
    }

    extension(XElement element)
    {
        /// <summary>Get the absolute XPath of the element.</summary>
        /// <remarks>
        /// e.g.: /people/person[6]/name[1]/last[1].
        /// </remarks>
        [Pure]
        internal string AbsoluteXPath()
        {
            var path = new StringBuilder();

            // A namespace is resolved by its nearest declaration at or above the
            // element, so the path is built from the root down while keeping the
            // in-scope prefix per namespace URI up to date.
            var prefixes = new Dictionary<string, string>(StringComparer.Ordinal);

            // If one element binds multiple prefixes to the same URI, only
            // the first binding is used.
            var declaredOnAncestor = new HashSet<string>(StringComparer.Ordinal);

            foreach (var ancestor in element.AncestorsAndSelf().Reverse())
            {
                declaredOnAncestor.Clear();

                foreach (var attr in ancestor.Attributes().Where(DeclaresPrefix).Where(a => declaredOnAncestor.Add(a.Value)))
                {
                    prefixes[attr.Value] = attr.Name.LocalName;
                }
                path.AppendRelativePath(ancestor, prefixes);
            }
            return path.ToString();

            static bool DeclaresPrefix(XAttribute attr) => attr is
            {
                IsNamespaceDeclaration: true,
                Name.Namespace.NamespaceName.Length: > 0,
            };
        }

        /// <summary>
        /// Get the index of the given XElement relative to its
        /// siblings with identical names. If the given element is
        /// the root, -1 is returned.
        /// </summary>
        [Pure]
        private int? Index()
        {
            if (element.Parent is { })
            {
                var index = 0;

                foreach (var sibling in element.Parent.Elements(element.Name))
                {
                    index++;
                    if (sibling == element) return index;
                }
                throw new InvalidOperationException("Element has been removed from its parent.");
            }
            else return null;
        }
    }

    extension(StringBuilder sb)
    {
        private void AppendRelativePath(XElement element, Dictionary<string, string> prefixes)
        {
            var name = element.Name.LocalName;

            if (prefixes.TryGetValue(element.Name.NamespaceName, out var prefix)
                && prefix is { Length: > 0 })
            {
                name = $"{prefix}:{name}";
            }
            sb.Append(element.Index() is { } index
                ? $"/{name}[{index}]"
                : $"/{name}");
        }
    }
}
