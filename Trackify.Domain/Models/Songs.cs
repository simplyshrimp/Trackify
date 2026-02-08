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
        public int albumId { get; set; }
        public int artistId { get; set; }
        public int timesPlayed { get; set; }
        public string filepath { get; set; }
        public Genres? genre { get; set; }
        public int albumTrackNr { get; set; }
        public bool isPrivate { get; set; }
        public int? playlistSongId { get; set; }
    }
}
