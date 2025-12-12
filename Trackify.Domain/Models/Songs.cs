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
    }
}
