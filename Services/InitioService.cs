using tablinumAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace tablinumAPI.Services
{
    public class InitioService
    {
        private readonly IMongoCollection<Initio> _initios;

        public InitioService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _initios = database.GetCollection<Initio>(settings.InitioCollectionName);
        }

        public List<Initio> Get() =>
            _initios.Find(initio => true).ToList();

        public Initio Get(string id) =>
            _initios.Find<Initio>(initio => initio.Id == id).FirstOrDefault();

        public Initio Create(Initio initio)
        {
            _initios.InsertOne(initio);
            return initio;
        }

        public void Update(string id, Initio initioIn) {
            initioIn.Id = id;
            _initios.ReplaceOne(initio => initio.Id == id, initioIn);
        }

        public void Remove(Initio initioIn) =>
            _initios.DeleteOne(initio => initio.Id == initioIn.Id);

        public void Remove(string id) => 
            _initios.DeleteOne(initio => initio.Id == id);
    }
}