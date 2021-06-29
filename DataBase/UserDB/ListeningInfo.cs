using System;
using MongoDB.Bson;

namespace WebPlayer.DataBase.UserDB
{
    public class ListeningInfo
    {
        public ObjectId TrackId { get; set; }
        public DateTime ListeningDate { get; set; }
        public int PauseTime { get; set; }

        public ListeningInfo(ObjectId trackId, DateTime listeningDate, int pauseTime)
        {
            TrackId = trackId;
            ListeningDate = listeningDate;
            PauseTime = pauseTime;
        }
    }
}