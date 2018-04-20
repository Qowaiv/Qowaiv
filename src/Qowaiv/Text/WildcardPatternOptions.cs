namespace Qowaiv.Text
{
    /// <summary>Describes the different wildcard pattern options.</summary>
    [System.Flags]
    public enum WildcardPatternOptions
    {
        /// <summary>No special options.</summary>
        None = 0,

        /// <summary>Use _ and % instead of ? and *.</summary>
        SqlWildcards = 1,
        
        /// <summary>The single char wildcard can also be trailing.</summary>
        SingleOrTrailing = 2,
    }
}
