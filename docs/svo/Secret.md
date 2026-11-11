# Secret
Represents a (text based) secret. It tries to avoid exposing potentially
sensitive strings to log files or (external) devices. It does not defend
against sources that have (direct) access to the system memory. Consider
if this is secure enough for the problem at hand, before using this type.

A secret can be created by parsing, deserializing JSON, or using its type
converter to convert from string. Serializing to JSON, or converting it
to another type are not supported, and `ToString()` returns `*****`. The
only way to access its value is by calling the `Value()` method.

``` C#
var secret = Secret.Parse("Ken sent me!");
var encrypted = secret.ComputeHash(sha512);
```
