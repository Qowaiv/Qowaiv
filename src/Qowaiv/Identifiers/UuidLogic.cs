namespace Qowaiv.Identifiers
{
    /// <summary>Implements <see cref="IIdentifierLogic"/> for an identifier based on <see cref="Uuid"/>.</summary>
    public abstract class UuidLogic : GuidLogic
    {
        internal new static readonly UuidLogic Instance = new Default();

        /// <summary>Gets the default format used to represent the <see cref="System.Guid"/> as <see cref="string"/>.</summary>
        protected override string DefaultFormat => "S";

        private class Default : UuidLogic { }
    }
}
