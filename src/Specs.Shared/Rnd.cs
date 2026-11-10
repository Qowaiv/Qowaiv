using System;
using System.Collections.Generic;
using System.Text;

namespace Specs;

public static class Rnd
{
#if NETSTANDARD2_0
    [ThreadStatic]
    public static readonly Random Shared = new Random();
#else
    public static readonly Random Shared = Random.Shared;
#endif
}
