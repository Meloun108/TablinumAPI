using tablinumAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace tablinumAPI.Services
{
    public class GroupService
    {
        private readonly IMongoCollection<Group> _groups;

        public GroupService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _groups = database.GetCollection<Group>(settings.GroupsCollectionName);
        }

        public List<Group> Get() =>
            _groups.Find(group => true).ToList();

        public Group Get(string id) =>
            _groups.Find<Group>(group => group.Id == id).FirstOrDefault();

        public Group Create(Group group)
        {
            _groups.InsertOne(group);
            return group;
        }

        public void Update(string id, Group groupIn) =>
            _groups.ReplaceOne(group => group.Id == id, groupIn);

        public void Remove(Group groupIn) =>
            _groups.DeleteOne(group => group.Id == groupIn.Id);

        public void Remove(string id) => 
            _groups.DeleteOne(group => group.Id == id);
    }
}