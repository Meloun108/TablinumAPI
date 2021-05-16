using tablinumAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace tablinumAPI.Services
{
    public class RoleService
    {
        private readonly IMongoCollection<Role> _roles;

        public RoleService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _roles = database.GetCollection<Role>(settings.RolesCollectionName);
        }

        public List<Role> Get() =>
            _roles.Find(role => true).ToList();

        public Role Get(string id) =>
            _roles.Find<Role>(role => role.Id == id).FirstOrDefault();

        public Role Create(Role role)
        {
            _roles.InsertOne(role);
            return role;
        }

        public void Update(string id, Role roleIn) =>
            _roles.ReplaceOne(role => role.Id == id, roleIn);

        public void Remove(Role roleIn) =>
            _roles.DeleteOne(role => role.Id == roleIn.Id);

        public void Remove(string id) => 
            _roles.DeleteOne(role => role.Id == id);
    }
}