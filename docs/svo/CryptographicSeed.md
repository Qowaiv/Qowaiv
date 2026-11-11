# Cryptographic Seed
Represents a byte array based cryptographic seed. It tries to avoid exposing
potentially sensitive data to log files or (external) devices. It does not
defend against sources that have (direct) access to the system memory. Consider
if this is secure enough for the problem at hand, before using this type.

A cryptographic seed can be created by parsing, deserializing JSON, or using
its type converter to convert from string. Serializing to JSON, or converting
it to another type are not supported, and `ToString()` returns `*****`. The
only way to access its value is by calling the `Value()` or `ToByteArray()` method.

``` C#
var seed = CryptographicSeed.Parse("S2VuIHNlbnQgbWUhIQ=="); // Base64 string
var seed = sha512.ComputeCryptographicSeed(new byte[]{ 0xD4, 0x1D, 0x8C, 0xD9 });
```
