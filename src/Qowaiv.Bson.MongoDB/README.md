# [MongoDB.Bson](https://mongodb.github.io/mongo-csharp-driver/)
MongoDB is a cross-platform document-oriented database program. Classified as a
NoSQL database program, MongoDB uses BSON documents with a schema. The .NET
library provides a mechanism to convert objects from and to BSON. To use
the `Qowaiv.Bson.MongoDB` package the following code can be used:

``` C#
 BsonSerializer.RegisterSerializationProvider(new QowaivBsonSerializationProvider());
```
