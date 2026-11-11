# UUID aka GUID
The UUID (Universally unique identifier), also known as GUID (Globally unique identifier), is an
extension of System.Guid. It is by default represented by a 22-character string, 
instead of a 32-character string.

``` C#
var rnd = Uuid.NewUuid();
UuidVersion version = rnd.Version; // UuidVersion.Random = 4

var bytes = Encoding.ASCII.GetBytes("Qowaiv");
var md5 = Uuid.GenerateWithMD5(bytes); //   lmZO_haEOTCwGsCcbIZFFg, UUID Version: 3
var sha1 = Uuid.GenerateWithSHA1(bytes); // 39h-Y1rR51ym_t78x9h0bA, UUID Version: 5
```

## Comparer
The UUID Comparer can sort both UUIDs and GUIDs. Furthermore, it supports both
.NET's default way of sorting as well as the sorting of SQL Server or MongoDB.

``` C#
var uuids = new List<Uuid>();
uuids.Sort(UuidComparer.SqlServer);

var uuids = new List<Uuid>();
uuids.Sort(UuidComparer.MongoDb);

var guids = new List<Guid>();
guids.Sort(UuidComparer.Default);
```

## Sequential
UUIDs are commonly used as the clustered key of a database table. For
massive databases with many inserts (which normally go hand in hand), this can
be a performance issue, as by default, generated UUIDs are not sequential, leading to
a lot of random inserts in the clustered index.

By using a sequential UUID, this problem can be minimized. Obviously, if you can
fully rely on the sequential UUID generation by your database of choice, you
should consider that, but in most cases you want to generate the ID upfront.
In that case, `Uuid.NewSequential()` comes in handy:

``` C#
var uuid = Uuid.NewSequential(UuidComparer.SqlServer);
```

As databases might (like SQL Server does) order your UUID/GUIDs differently
than .NET does, this generator does that too. Also keep in mind that this generated ID
is not perfectly sequential; first of all because it has a 0.32-nanosecond
overlap, but more seriously, because some time may elapse between generation and
storage in the database. Furthermore, these generated UUIDs are not sequential
once mixed with sequentially generated UUIDs by your database.
