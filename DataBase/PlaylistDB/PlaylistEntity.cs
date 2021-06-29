using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlayer.DataBase.PlaylistDB
{
    public class PlaylistEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public List<ObjectId> Tracks { get; set; }
        public int Rating { get; set; }

        public PlaylistEntity(string title, string creator, List<ObjectId> tracks)
        {
            Title = title;
            Creator = creator;
            Tracks = tracks;
            Rating = 0;
        }
    }
}