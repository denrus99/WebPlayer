using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlayer.DataBase.AlbumDB
{
    public class AlbumEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public List<ObjectId> Singers { get; set; }
        public List<ObjectId> Tracks { get; set; }
        public int Rating { get; set; }
        //TODO: поменять строку на байты
        public string Photo { get; set; }

        public AlbumEntity(string title, int releaseYear, List<ObjectId> singers, List<ObjectId> tracks)
        {
            Title = title;
            ReleaseYear = releaseYear;
            Singers = singers;
            Tracks = tracks;
            Rating = 0;
        }
    }
}