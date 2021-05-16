using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace tablinumAPI.Models
{
    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Number")]
        [JsonProperty("Number")]
        public string DocumentNumber { get; set; }

        public DateTime NumberDate { get; set; }

        public string NumberCenter { get; set; }

        public DateTime NumberCenterDate { get; set; }
        public string NumberDepartment { get; set; }
        public DateTime NumberDepartmentDate { get; set; }
        //public string NumberGroup { get; set; }
        //public DateTime NumberGroupDate { get; set; }
        public List<DocumentGroup> GroupInfo { get; set; }
        //public string From { get; set; }
        //public Initio From { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("From")]
        [JsonProperty("From")]
        public string InitioId { get; set; }
        //public string Executor { get; set; }
        //public User Executor { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Executor")]
        [JsonProperty("Executor")]
        public string UserId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public bool Status { get; set; }
        public string View { get; set; }
        public string Speed { get; set; }
        public bool Control { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        //public List<DateTime> Updated { get; set; }
    }
}