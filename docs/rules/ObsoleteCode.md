# CS0618 & CS0619: Obsolete member
The compiler will report [CS0612](https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0612),
[CS0618](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0618),
and [CS0619](https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0619)]
for calls to code that is marked as being obsolete:

``` C#
[Obsolete]                                   // CS0612
[Obsolete("Use newMethod instead.", false)]  // CS0618
[Obsolete("Use newMethod instead.", true)]   // CS0619
```

This code fix can provide a suggestion for both CS0618, and CS0619 if the
message follows the pattern `Use (?<Suggestion>.+) instead`.

In that case the obsolete member call is replaced by the suggestion.
