using tablinumAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using tablinumAPI.Controllers;

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
            var docs = _documents.Find(Filter).ToList();
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