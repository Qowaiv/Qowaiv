using System;
using System.Collections.Generic;
using System.Text;

namespace Specs;

public static class Rnd
{
#if NETSTANDARD2_0
#pragma warning disable S2245 // Pseudorandom number generators (PRNGs) should not be used in security contexts
    public static readonly Random Shared = new Random();
#pragma warning restore S2245 // Pseudorandom number generators (PRNGs) should not be used in security contexts
#else
    public static readonly Random Shared = Random.Shared;
#endif
}
