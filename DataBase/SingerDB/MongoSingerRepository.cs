using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPlayer.DataBase.Settings;

namespace WebPlayer.DataBase.SingerDB
{
    public class MongoSingerRepository : IEntityRepository<SingerEntity>
    {
        private SingersDataBaseSettings _settings;
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoSingerRepository(SingersDataBaseSettings settings)
        {
            _settings = settings;
            _client = new MongoClient(_settings.ConnectionString);
            _database = _client.GetDatabase(_settings.DatabaseName);
        }
        //TODO: Проверить все методы singerrepository

        public async Task Insert(SingerEntity entity)
        {
            var collection = _database.GetCollection<SingerEntity>(_settings.CollectionName);
            await collection.InsertOneAsync(entity);
        }

        public SingerEntity Find(string id)
        {
            return Find(new ObjectId(id));
        }

        public ReplaceOneResult Update(SingerEntity entity)
        {
            var collection = _database.GetCollection<SingerEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", entity.Id);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions());
        }

        public SingerEntity Find(ObjectId id)
        {
            var collection = _database.GetCollection<SingerEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", id);
            return collection.Find(filter).ToList().FirstOrDefault();
        }

        public DeleteResult Delete(string id)
        {
            var collection = _database.GetCollection<SingerEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", new ObjectId(id));
            return collection.DeleteOne(filter);
        }
    }
}