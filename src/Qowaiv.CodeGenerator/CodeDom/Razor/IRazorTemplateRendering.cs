namespace Qowaiv.CodeGenerator.CodeDom.Razor
{
    /// <summary>Interface for razor template rendering.</summary>
    public interface IRazorTemplateRendering
    {
        /// <summary>Renders a model.</summary>
        /// <param name="model">
        /// The model to render.
        /// </param>
        /// <param name="postfix">
        /// The optional postfix for finding the template.
        /// </param>
        string RenderModel(object model, string postfix = null);

        /// <summary>Renders a snippet.</summary>
        /// <param name="name">
        /// The name of the snippet.
        /// </param>
        string RenderSnippet(string name);
    }
}
