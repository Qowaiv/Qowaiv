﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Qowaiv.Globalization
{
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
        #region Constructors

        /// <summary>Creates a new CultureInfo scope.</summary>
        /// <remarks>
        /// No direct access.
        /// </remarks>
        protected CultureInfoScope() { }

        /// <summary>Creates a new CultureInfo scope.</summary>
        /// <param name="name">
        /// Name of the culture.
        /// </param>
        public CultureInfoScope(string name) : this(name, name) { }

        /// <summary>Creates a new CultureInfo scope.</summary>
        /// <param name="name">
        /// Name of the culture.
        /// </param>
        /// <param name="nameUI">
        /// Name of the UI culture.
        /// </param>
        public CultureInfoScope(string name, string nameUI)
            : this(new CultureInfo(Guard.NotNullOrEmpty(name, "name")), new CultureInfo(Guard.NotNullOrEmpty(nameUI, "nameUI"))) { }

        /// <summary>Creates a new CultureInfo scope.</summary>
        /// <param name="culture">
        /// The culture.
        /// </param>
        public CultureInfoScope(CultureInfo culture)
            : this(culture, culture) { }

        /// <summary>Creates a new CultureInfo scope.</summary>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <param name="cultureUI">
        /// The UI culture.
        /// </param>
        public CultureInfoScope(CultureInfo culture, CultureInfo cultureUI)
        {
            Guard.NotNull(culture, "culture");
            Guard.NotNull(cultureUI, "cultureUI");

            Previous = Thread.CurrentThread.CurrentCulture;
            PreviousUI = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = cultureUI;
        }

        #endregion

        /// <summary>Gets the previous Current UI Culture.</summary>
        public CultureInfo PreviousUI { get; private set; }
        /// <summary>Gets the previous Current Culture.</summary>
        public CultureInfo Previous { get; private set; }

        /// <summary>Represents the CultureInfo scope as <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string DebuggerDisplay
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}: [{1}/{2}], Previous: [{3}/{4}]",
                    GetType().Name,
                    Thread.CurrentThread.CurrentCulture.Name,
                    Thread.CurrentThread.CurrentUICulture.Name,
                    Previous.Name,
                    PreviousUI.Name);
            }
        }

        #region Factory Methods

        /// <summary>Gets a new invariant CultureInfo Scope.</summary>
        public static CultureInfoScope NewInvariant()
        {
            return new CultureInfoScope(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture);
        }

        #endregion

        #region IDisposable

        /// <summary></summary>
        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass 
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        /// <summary>Disposes the scope by setting the privious cultures back.</summary>
        /// <param name="disposing">
        /// Should dispose actualy dispose something.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                Thread.CurrentThread.CurrentCulture = Previous;
                Thread.CurrentThread.CurrentUICulture = PreviousUI;
                Disposed = false;
            }
        }

        /// <summary>Gets and set if the CultureInfo Scope is disposed.</summary>
        protected bool Disposed { get; set; }

        #endregion
    }
}
