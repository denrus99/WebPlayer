using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlayer.DataBase.TrackDB
{
    public class TrackEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Singer { get; set; }
        public bool IsPodcast { get; set; }
        public string Text { get; set; }
        public ObjectId FileId { get; set; }
        public int Rating { get; set; }

        public TrackEntity(ObjectId trackId, string title, string singer, bool isPodcast, string text, ObjectId fileId)
        {
            Id = trackId;
            Title = title;
            Singer = singer;
            IsPodcast = isPodcast;
            FileId = fileId;
            Text = text;
            Rating = 0;
        }
    }
}