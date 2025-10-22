#pragma warning disable QW0015 // Define global using statements in single file
#if NET9_0_OR_GREATER
global using Lock = System.Threading.Lock;
#else
global using Lock = System.Object;
#endif
