using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPlayer.DataBase.Settings;

namespace WebPlayer.DataBase.AlbumDB
{
    public class MongoAlbumRepository : IEntityRepository<AlbumEntity>
    {
        private AlbumsDataBaseSettings _settings;
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoAlbumRepository(AlbumsDataBaseSettings settings)
        {
            _settings = settings;
            _client = new MongoClient(_settings.ConnectionString);
            _database = _client.GetDatabase(_settings.DatabaseName);
        }
        //TODO: Проверить все методы albumrepository

        public async Task Insert(AlbumEntity entity)
        {
            var collection = _database.GetCollection<AlbumEntity>(_settings.CollectionName);
            await collection.InsertOneAsync(entity);
        }

        public AlbumEntity Find(string objectId)
        {
            return Find(new ObjectId(objectId));
        }

        public AlbumEntity Find(ObjectId objectId)
        {
            var collection = _database.GetCollection<AlbumEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", objectId);
            return collection.Find(filter).ToList().FirstOrDefault();
        }

        public async Task<AlbumEntity> FindByTrack(ObjectId objectId)
        {
            var collection = _database.GetCollection<AlbumEntity>("Albums");
            var filter = Builders<AlbumEntity>.Filter.Where(
                albumEntity => albumEntity.Tracks.Contains(objectId)
            );
            var cursor = await collection.FindAsync(filter);
            await cursor.MoveNextAsync();
            var albumEntity = cursor.Current.FirstOrDefault();
            return albumEntity;
        }

        public ReplaceOneResult Update(AlbumEntity entity)
        {
            var collection = _database.GetCollection<AlbumEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", entity.Id);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions());
        }

        public DeleteResult Delete(string id)
        {
            var collection = _database.GetCollection<AlbumEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", new ObjectId(id));
            return collection.DeleteOne(filter);
        }
    }
}