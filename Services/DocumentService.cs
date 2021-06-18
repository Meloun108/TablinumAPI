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

        public void UpdateLocation(string id, string[] locationsID) {
            foreach (var loc in locationsID) {
                var filter = Builders<Document>.Filter.Eq(x => x.Id, id) & Builders<Document>.Filter.ElemMatch(x => x.GroupInfo, 
                                                                       Builders<DocumentGroup>.Filter.Eq(x => x.GroupId, loc));
                var update = Builders<Document>.Update.Set(x => x.GroupInfo[-1].Location, false);
                _documents.UpdateOneAsync(filter, update);
            }
        }

        public void AddNewLocation(string id, DocumentGroup documentGroup) {
            var filter = Builders<Document>.Filter.Eq(x => x.Id, id);
            var update = Builders<Document>.Update.AddToSet(x => x.GroupInfo, documentGroup);
            _documents.UpdateOneAsync(filter, update);
        }

        public void UpdateNumberLocation(string id, List<DocumentGroup> documentGroup) {
            //var filter = Builders<Document>.Filter.Eq(x => x.Id, id);
            //var update = Builders<Document>.Update.Set("GroupInfo.$[i].Group", documentGroup.GroupId );
            //var arrayFilters = new List<ArrayFilterDefinition> { new JsonArrayFilterDefinition<DocumentGroup>("{'i.Group': 
            //                                                     { $in: ["+string.Join(",", documentGroup.GroupId.Select(s => string.Format("\"{0}\"", s)).ToArray())+"]}}") };
            //var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
            //var result = _documents.UpdateOne(filter, update, updateOptions);
            //var filter = Builders<Document>.Filter.Eq(x => x.Id, id);
            //var update = Builders<Document>.Update.AddToSet(x => x.GroupInfo, documentGroup);
            //_documents.UpdateOneAsync(filter, update);
            //var filter = Builders<Document>.Filter.Eq(x => x.Id, id) & Builders<Document>.Filter.ElemMatch(x => x.GroupInfo, 
            //                                                           Builders<DocumentGroup>.Filter.Eq(x => x.GroupId, documentGroup.GroupId));
            //var update = Builders<Document>.Update.Set(x => x.GroupInfo[-1].NumberGroup, documentGroup.NumberGroup);
            //_documents.UpdateOneAsync(filter, update);
            var filter = Builders<Document>.Filter.Eq(x => x.Id, id);
            var update = Builders<Document>.Update.Set(x => x.GroupInfo, documentGroup);
            _documents.UpdateOneAsync(filter, update);
        }

        public void DeleteLocation(string id, List<DocumentGroup> documentGroups) {
            var filter = Builders<Document>.Filter.Eq(x => x.Id, id);
               var update = Builders<Document>.Update.Set(x => x.GroupInfo, documentGroups);
            _documents.UpdateOneAsync(filter, update);
        }

        public void Remove(Document documentIn) =>
            _documents.DeleteOne(document => document.Id == documentIn.Id);

        public void Remove(string id) => 
            _documents.DeleteOne(document => document.Id == id);
    }
}