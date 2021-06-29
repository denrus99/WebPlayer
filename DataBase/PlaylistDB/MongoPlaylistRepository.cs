using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPlayer.DataBase.Settings;

namespace WebPlayer.DataBase.PlaylistDB
{
    public class MongoPlaylistRepository : IEntityRepository<PlaylistEntity>
    {
        private PlaylistsDataBaseSettings _settings;
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoPlaylistRepository(PlaylistsDataBaseSettings settings)
        {
            _settings = settings;
            _client = new MongoClient(_settings.ConnectionString);
            _database = _client.GetDatabase(_settings.DatabaseName);
        }
        //TODO: Проверить все методы playlistrepository

        public async Task Insert(PlaylistEntity entity)
        {
            var collection = _database.GetCollection<PlaylistEntity>(_settings.CollectionName);
            await collection.InsertOneAsync(entity);
        }

        public PlaylistEntity Find(string id)
        {
            return Find(new ObjectId(id));
        }

        public PlaylistEntity Find(ObjectId id)
        {
            var collection = _database.GetCollection<PlaylistEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", id);
            return collection.Find(filter).ToList().FirstOrDefault();
        }

        public List<PlaylistEntity> FindPopular(int count)
        {
            var collection = _database.GetCollection<PlaylistEntity>(_settings.CollectionName);
            var sortDefinition = Builders<PlaylistEntity>.Sort.Descending(playlist => playlist.Rating);
            var list = collection.Find(new BsonDocument()).Sort(sortDefinition).Limit(count).ToList();
            return list;
        }

        public ReplaceOneResult Update(PlaylistEntity entity)
        {
            var collection = _database.GetCollection<PlaylistEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", entity.Id);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions());
        }

        public DeleteResult Delete(string id)
        {
            var collection = _database.GetCollection<PlaylistEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", new ObjectId(id));
            return collection.DeleteOne(filter);
        }
    }
}