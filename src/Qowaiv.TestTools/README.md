# Qowaiv.TestTools

## Domain-driven design bottom up
Qowaiv is a (Single) Value Object library. It aims to model reusable, immutable,
(Single) Value Objects that can be used a wide variety of modeling scenarios,
both inside and outside a Domain-driven context.

## Package
This package contains helpers to make writing unit tests for SVO's easier.

## IO

### Temporary directory
Testing IO can be cumbersome. The `TemporaryDirectory` can help by creating a
directory that only exists for the duration of a test:

``` C#
using (var directory = new TemporaryDirectory()
{
     var file = directory.CraeteFile("somefile.txt");
     // ..
}
```

On the dispose, the directory, and all it children will be deleted.

### File lock
To test IO related unhappy flows, it can be useful to create a temporary lock
on a file. This can be done as follows:

``` C#

var file = new FileInfo("somefile.txt");

using (var @lock = file.Lock())
{
     file.OpenRead(); // Throws IOException.
}
```

## Further reading
More info can be found at https://github.com/Qowaiv/Qowaiv.
