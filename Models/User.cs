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
        public string Salt { get; set; }
        //public Group Group { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Group")]
        [JsonProperty("Group")]
        public string GroupId { get; set; }
        public string Name { get; set; }
        //public Role Role { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Role")]
        [JsonProperty("Role")]
        public string RoleId { get; set; }
    }
}