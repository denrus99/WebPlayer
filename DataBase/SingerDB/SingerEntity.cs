using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlayer.DataBase.SingerDB
{
    public class SingerEntity
    {
        [BsonId] 
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<ObjectId> Albums { get; set; }
        public int Rating { get; set; }
        //TODO: нужно ли менять строку на байты
        public string Photo { get; set; }
        public List<ObjectId> Playlists { get; set; }
        public List<ObjectId> Tracks { get; set; }
        

        public SingerEntity(string name, List<ObjectId> albums, string photo)
        {
            Name = name;
            Albums = albums;
            Photo = photo;
            Rating = 0;
            Playlists = new List<ObjectId>();
            Tracks = new List<ObjectId>();
        }
    }
}