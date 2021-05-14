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
        public string NumberGroup { get; set; }
        public DateTime NumberGroupDate { get; set; }
        //public List<DocumentGroup> GroupInfo { get; set; }
        public string From { get; set; }
        //public Initio From { get; set; }
        public string Executor { get; set; }
        //public User Executor { get; set; }
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


/*namespace tablinumAPI.Models
{
    public class TablinumItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public string Secret { get; set; }
    }

    public class TablinumItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}*/