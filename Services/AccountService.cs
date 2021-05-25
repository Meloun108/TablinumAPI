using tablinumAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System;
using System.Text;

namespace tablinumAPI.Services
{
    public class AccountService
    {
        private readonly IMongoCollection<User> _users;

        public AccountService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string login) =>
            _users.Find<User>(user => user.UserLogin == login).FirstOrDefault();

        public User Create(User user)
        {
            using(var sha256 = SHA256.Create())  
            {
                string salt = "";
                byte[] bytes = new byte[128 / 8];
                using(var keyGenerator = RandomNumberGenerator.Create())
                {
                    keyGenerator.GetBytes(bytes);
                    salt = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password + salt));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                user.Password = hash;
                user.Salt = salt;
            }
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) => 
            _users.DeleteOne(user => user.Id == id);
    }
}