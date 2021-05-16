using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace tablinumAPI.Models
{
    public class Initio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Initio")]
        [JsonProperty("Initio")]
        public string Executor { get; set; }
    }
}