using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.Domain.Models
{
    public class Songs
    {
        public int songId { get; set; }
        public TimeSpan length {  get; set; }
        public string title { get; set; }
        public int albumID { get; set; } //maybe make sql image to just get albumcover for the song
        public int artistID { get; set; }
        public int timesPlayed { get; set; }
        public string filepath { get; set; }
        public bool isPrivate { get; set; }

        public Songs(int songID, TimeSpan length, string title, int albumID, int artistID, int timesPlayed, string filepath, bool isPrivate)
        {
            this.songId = songID;
            this.length = length;
            this.title = title;
            this.albumID = albumID;
            this.artistID = artistID;
            this.timesPlayed = timesPlayed;
            this.filepath = filepath;
            this.isPrivate = isPrivate;
        }
    }
}
