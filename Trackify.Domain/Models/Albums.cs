using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models.Enums;

namespace Trackify.Domain.Models
{
    public class Albums
    {
        public int albumId { get; set; }
        public string albumTitle { get; set; }
        public AlbumType albumType { get; set; }
        public Artists artist { get; set; }
        public string albumImage { get; set; }
        public bool madePrivate { get; set; }
        public string color { get; set; }
        public List<Songs> songs { get; set; }
    }
}
