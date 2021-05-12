using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace tablinumAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Login")]
        [JsonProperty("Login")]
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}