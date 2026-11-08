# Qowaiv.Bson.MongoDB
[MongoDB](https://www.mongodb.com/docs/drivers/csharp/) is a cross-platform
document-oriented database program. Classified as a NoSQL database program,
MongoDB uses BSON documents with a schema. The .NET library provides a
mechanism to convert objects from and to BSON.

## Installation
``` bash
dotnet add package Qowaiv.Bson.MongoDB
```

## Register the converter
To globally register the converter:

``` C#
BsonSerializer.RegisterSerializationProvider(new QowaivBsonSerializationProvider());
```
