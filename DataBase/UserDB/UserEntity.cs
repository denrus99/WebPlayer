using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlayer.DataBase.UserDB
{
    public class UserEntity
    {
        [BsonId] public string Login { get; set; }
        public string Password { get; set; }
        //TODO: нужно ли менять строку на байты
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public List<ListeningInfo> History { get; set; }
        public List<ObjectId> Albums { get; set; }
        public List<ObjectId> Tracks { get; set; }
        public List<ObjectId> Playlists { get; set; }
        public List<ObjectId> Singers { get; set; }

        public UserEntity(string login, string password, string email, string name, string role, List<ListeningInfo> history)
        {
            Login = login;
            Password = password;
            Email = email;
            Name = name;
            Role = role;
            History = history;
            Albums = new List<ObjectId>();
            Playlists = new List<ObjectId>();
            Tracks = new List<ObjectId>();
            Singers = new List<ObjectId>();
        }
        
        public UserEntity(string login, string password, string email, string name)
        {
            Login = login;
            Password = password;
            Email = email;
            Name = name;
            Role = "Subscriber";
            History = new List<ListeningInfo>();
            Albums = new List<ObjectId>();
            Playlists = new List<ObjectId>();
            Tracks = new List<ObjectId>();
            Singers = new List<ObjectId>();
        }
    }
}