# Stream size
Represents the size of a file or stream.

``` C#
StreamSize fromMb = StreamSize.FromMegabytes(14.2);
StreamSize parsed = StreamSize.Parse("117.2Kb");
StreamSize humanizer = 142.23.KB(); // Bytes(), KiB(), MB(), MiB(), GB(), GiB()

// Short notation
new StreamSize(8900).ToString("s") => 8900b
new StreamSize(238900).ToString("s") => 238.9kb
new StreamSize(238900).ToString(" S") => 238.9 kB
new StreamSize(238900).ToString("0000.00 S") => 0238.90 kB

// Full notation
new StreamSize(8900).ToString("0.0 f") => 8900.0 byte
new StreamSize(238900).ToString("0 f") => 239 kilobyte
new StreamSize(1238900).ToString("0.00 F") => 1.24 Megabyte

// Custom
new StreamSize(8900).ToString("0.0 kb") => 8.9 kb
new StreamSize(238900).ToString("0.0 MB") => 0.2 MB
new StreamSize(1238900).ToString("#,##0.00 Kilobyte") => 1,239.00 Kilobyte
new StreamSize(1238900).ToString("#,##0") => 1,238,900
```
