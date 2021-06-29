using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using WebPlayer.DataBase.Settings;

namespace WebPlayer.DataBase.TrackDB
{
    public class MongoTrackRepository : IEntityRepository<TrackEntity>
    {
        private TracksDataBaseSettings _settings;
        private MongoClient _client;
        private IMongoDatabase _database;
        private string _bucketName;
        
        public MongoTrackRepository(TracksDataBaseSettings settings)
        {
            _settings = settings;
            _client = new MongoClient(_settings.ConnectionString);
            _database = _client.GetDatabase(_settings.DatabaseName);
            _bucketName = "audios";
        }
        
        public async Task Insert(TrackEntity entity)
        {
            var collection = _database.GetCollection<TrackEntity>(_settings.CollectionName);
            await collection.InsertOneAsync(entity);
        }
        
        public TrackEntity Find(string id)
        {
            return Find(new ObjectId(id));
        }
        
        public TrackEntity Find(ObjectId id)
        {
            var collection = _database.GetCollection<TrackEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", id);
            return collection.Find(filter).ToList().FirstOrDefault();
        }
        
        public byte[] DownloadAudio(string id)
        {
            var gridFs = new GridFSBucket(_database, new GridFSBucketOptions()
            {
                BucketName = _bucketName
            });
            return gridFs.DownloadAsBytes(new ObjectId(id));
        }
        
        public ReplaceOneResult Update(TrackEntity entity)
        {
            var collection = _database.GetCollection<TrackEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", entity.Id);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions());
        }

        public DeleteResult Delete(string id)
        {
            var collection = _database.GetCollection<TrackEntity>(_settings.CollectionName);
            var filter = new BsonDocument("_id", new ObjectId(id));
            return collection.DeleteOne(filter);
        }
    }
}