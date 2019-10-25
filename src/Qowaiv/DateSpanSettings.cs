#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
// Default functionally describes best what is going on: the default settings are used.
using System;

namespace Qowaiv
{
    /// <summary>Setting used the calculate a <see cref="DateSpan"/>.</summary>
    [Flags]
    public enum DateSpanSettings
    {
        /// <summary>Default (years, months, days, total months first, and no mixed signs).</summary>
        Default = 0,

        /// <summary>Without years (only has impact if also without months).</summary>
        WithoutYears = 1,

        /// <summary>Without months (example: +20Y+0M+300D).</summary>
        /// <remarks>
        /// Used in <see cref="DateSpan.Age(Date)"/>.
        /// </remarks>
        WithoutMonths = 2,

        /// <summary>Allow mixed signs (example: 4Y+3M-2D).</summary>
        MixedSigns = 4,

        /// <summary>The differences in days is calculated first.</summary>
        DaysFirst = 8,

        /// <summary>The calculated date span is defined in days only.</summary>
        DaysOnly = Default ^ (WithoutMonths | WithoutYears),
    }
}
