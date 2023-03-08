using System.Threading;

namespace Qowaiv.Globalization;

/// <summary>Handles the CultureInfo of a thread during its scope.</summary>
/// <remarks>
/// <code>
/// using(new CultureInfoScope("be-NL"))
/// {
///     // Do things.
/// }
/// </code>
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay}")]
public class CultureInfoScope : IDisposable
{
    /// <summary>Initializes a new instance of the <see cref="CultureInfoScope"/> class.</summary>
    /// <remarks>
    /// No direct access.
    /// </remarks>
    private CultureInfoScope()
    {
        Previous = Thread.CurrentThread.CurrentCulture;
        PreviousUI = Thread.CurrentThread.CurrentUICulture;
    }

    /// <summary>Initializes a new instance of the <see cref="CultureInfoScope"/> class.</summary>
    /// <param name="culture">
    /// The culture.
    /// </param>
    /// <param name="cultureUI">
    /// The UI culture.
    /// </param>
    public CultureInfoScope(CultureInfo culture, CultureInfo cultureUI) : this()
    {
        Thread.CurrentThread.CurrentCulture = Guard.NotNull(culture, nameof(culture));
        Thread.CurrentThread.CurrentUICulture = Guard.NotNull(cultureUI, nameof(cultureUI));
    }

    /// <summary>Initializes a new instance of the <see cref="CultureInfoScope"/> class.</summary>
    /// <param name="name">
    /// Name of the culture.
    /// </param>
    /// <param name="nameUI">
    /// Name of the UI culture.
    /// </param>
    public CultureInfoScope(string name, string nameUI) : this(
        new CultureInfo(Guard.NotNullOrEmpty(name, nameof(name))),
        new CultureInfo(Guard.NotNullOrEmpty(nameUI, nameof(nameUI)))) { }

    /// <summary>Initializes a new instance of the <see cref="CultureInfoScope"/> class.</summary>
    /// <param name="name">
    /// Name of the culture.
    /// </param>
    public CultureInfoScope(string name) : this(name, name) { }

    /// <summary>Initializes a new instance of the <see cref="CultureInfoScope"/> class.</summary>
    /// <param name="culture">
    /// The culture.
    /// </param>
    public CultureInfoScope(CultureInfo culture) : this(culture, culture) { }

    /// <summary>Gets the previous Current UI Culture.</summary>
    public CultureInfo PreviousUI { get; }

    /// <summary>Gets the previous Current Culture.</summary>
    public CultureInfo Previous { get; }

    /// <summary>Represents the CultureInfo scope as <see cref="string"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected string DebuggerDisplay
        => string.Format(
            CultureInfo.InvariantCulture,
            "{0}: [{1}/{2}], Previous: [{3}/{4}]",
            GetType().Name,
            Thread.CurrentThread.CurrentCulture.Name,
            Thread.CurrentThread.CurrentUICulture.Name,
            Previous.Name,
            PreviousUI.Name);

    /// <summary>Gets a new invariant CultureInfo Scope.</summary>
    [Impure]
    public static CultureInfoScope NewInvariant()
        => new(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Disposes the scope by setting the previous cultures back.</summary>
    /// <param name="disposing">
    /// Should dispose actually dispose something.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed && disposing)
        {
            Thread.CurrentThread.CurrentCulture = Previous;
            Thread.CurrentThread.CurrentUICulture = PreviousUI;
            Disposed = true;
        }
    }

    /// <summary>Gets and set if the CultureInfo Scope is disposed.</summary>
    protected bool Disposed;
}
