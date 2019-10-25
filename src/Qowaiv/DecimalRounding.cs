namespace Qowaiv
{
    /// <summary>Methods of rounding <see cref="decimal"/>s.</summary>
    /// <remarks>
    /// This is an extension on <see cref="System.MidpointRounding"/>.
    /// </remarks>
    public enum DecimalRounding
    {
        /// <summary>When a number is halfway between two others, it is rounded toward the nearest even.</summary>
        ToEven = 0,

        /// <summary>Bankers round, also known as <see cref="ToEven"/>.</summary>
        BankersRound = ToEven,

        /// <summary>When a number is halfway between two others, it is rounded toward the nearest number that is away from zero.</summary>
        AwayFromZero = 1,

        /// <summary>When a number is halfway between two others, it is rounded toward the nearest odd.</summary>
        ToOdd = 2,

        /// <summary>When a number is halfway between two others, it is rounded toward the nearest number that is closest to zero.</summary>
        TowardsZero = 3,

        /// <summary>When a number is halfway between two others, it is rounded toward the highest of the two.</summary>
        Up = 4,
        
        /// <summary>When a number is halfway between two others, it is rounded toward the lowest of the two.</summary>
        Down = 5,

        /// <summary>When a number is halfway between two others, it is randomly rounded up or down with equal probability.</summary>
        RandomTieBreaking = 6,

        /// <summary>When a number is between two others, the remainder is truncated/ignored.</summary>
        Truncate = 7,

        /// <summary>When a number is between two others, it is rounded toward the nearest number that is away from zero.</summary>
        DirectAwayFromZero = 8,

        /// <summary>When a number is between two others, it is rounded toward the nearest number that is closest to zero.</summary>
        DirectTowardsZero = 9,

        /// <summary>When a number is between two others, its rounded to the largest.</summary>
        Ceiling = 10,

        /// <summary>When a number is between two others, its rounded to the smallest.</summary>
        Floor = 11,

        /// <summary>When a number is between two others, it is randomly rounded up or down with stochastic probability.</summary>
        StochasticRounding = 12,
    }
}
