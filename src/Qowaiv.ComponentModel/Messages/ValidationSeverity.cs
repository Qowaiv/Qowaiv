namespace Qowaiv.ComponentModel.Messages
{
    /// <summary>The validation severity of a <see cref="ValidationMessage"/>.</summary>
    public enum ValidationSeverity
    {
        /// <summary>Successful message (so no severity).</summary>
        None = -1,

        /// <summary>Informative message severity.</summary>
        Info = 0,
        /// <summary>Warning message severity.</summary>
        Warning = 1,

        /// <summary>Error message severity.</summary>
        Error = 2,
    }
}
