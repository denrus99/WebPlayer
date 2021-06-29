using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPlayer.Controllers;
using WebPlayer.DataBase.Settings;

namespace WebPlayer.DataBase.UserDB
{
    public class MongoUserRepository : IEntityRepository<UserEntity>
    {
        private UsersDataBaseSettings _settings;
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoUserRepository(UsersDataBaseSettings settings)
        {
            _settings = settings;
            _client = new MongoClient(_settings.ConnectionString);
            _database = _client.GetDatabase(_settings.DatabaseName);
        }
        
        public async Task Insert(UserEntity entity)
        {
            var collection = _database.GetCollection<UserEntity>(_settings.CollectionName);
            await collection.InsertOneAsync(entity);
        }

        public UserEntity Find(ObjectId objectId)
        {
            var collection = _database.GetCollection<UserEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", objectId.ToString());
            return collection.Find(filter).ToList().FirstOrDefault();
        }
        public UserEntity Find(string login)
        {
            var collection = _database.GetCollection<UserEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", login);
            return collection.Find(filter).ToList().FirstOrDefault();
        }

        public ReplaceOneResult Update(UserEntity entity)
        {
            var collection = _database.GetCollection<UserEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", entity.Login);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions());
        }

        public DeleteResult Delete(string login)
        {
            var collection = _database.GetCollection<UserEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", login);
            return collection.DeleteOne(filter);
        }
    }
}