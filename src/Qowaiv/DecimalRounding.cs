namespace Qowaiv
{
    /// <summary>Methods of rounding <see cref="decimal"/>s.</summary>
    public enum DecimalRounding
    {
        /// <summary>When a number is halfway between two others, it is rounded toward the nearest.</summary>
        ToEven = 0,

        /// <summary>Bankers round, also known as <see cref="ToEven"/>.</summary>
        BankersRound = ToEven,

        /// <summary>When a number is halfway between two others, it is rounded toward the nearest number that is away from zero.</summary>
        AwayFromZero = 1,

        /// <summary>When a number is halfway between two others, its rounded to the smallest.</summary>
        Floor = 2,
        /// <summary>When a number is halfway between two others, its rounded to the largest.</summary>
        Ceiling = 3,

        /// <summary>When a number is halfway between two others, that part is truncated.</summary>
        Truncate = 4,
    }
}
