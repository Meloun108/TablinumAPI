using tablinumAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace tablinumAPI.Services
{
    public class DocumentService
    {
        private readonly IMongoCollection<Document> _documents;

        public DocumentService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _documents = database.GetCollection<Document>(settings.DocumentsCollectionName);
        }

        public List<Document> Get() =>
            _documents.Find(document => true).ToList();

        //public List<Document> GetUserDoc(string group)  =>
        //    _documents.Find<Document>(document => document.GroupInfo.Group == group).ToList();

        public List<Document> GetUserDoc(string group) {
            var Filter = new BsonDocument("GroupInfo.Group", ObjectId.Parse(group));
            //var Filter = Builders<BsonDocument>.Filter.Gt("GroupInfo._id", group);
            var docs = _documents.Find(Filter).ToList();
            //var docs = _documents.Aggregate()
                // filter the ConnectingQuestions array to only include the element with QuestionNumber == 2
            //    .Project<Document>("{ 'GroupInfo': { $filter: { input: '$GroupInfo', cond: { $eq: [ '$$this.Group', '" + group + "' ] } } } }")
                // move the first (single!) item from "ConnectingQuestions" array to the document root level
            //    .ToList();
            return docs;
        }

        public Document Get(string id) =>
            _documents.Find<Document>(document => document.Id == id).FirstOrDefault();

        public Document Create(Document document)
        {
            _documents.InsertOne(document);
            return document;
        }

        public void Update(string id, Document documentIn) =>
            _documents.ReplaceOne(document => document.Id == id, documentIn);

        public void Remove(Document documentIn) =>
            _documents.DeleteOne(document => document.Id == documentIn.Id);

        public void Remove(string id) => 
            _documents.DeleteOne(document => document.Id == id);
    }
}