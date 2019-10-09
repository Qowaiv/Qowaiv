using System;

namespace Qowaiv
{
    [Flags]
    public enum DateSpanSettings
    {
        Default = 0,
        WithoutYears = 1,
        WithoutMonths = 2,
        MixedSigns = 4,
        DaysFirst = 8,
        DaysOnly = Default ^ (WithoutMonths | WithoutYears),
    }
}
