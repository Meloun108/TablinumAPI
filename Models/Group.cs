using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace tablinumAPI.Models
{
    public class Group
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Dept")]
        [JsonProperty("Dept")]
        public string GroupName { get; set; }
    }
    public class DocumentGroup
    {
        public Group Group { get; set; }
        public string NumberGroup { get; set; }
        public DateTime NumberGroupDate { get; set; }
        public bool Location { get; set; }
    }
}