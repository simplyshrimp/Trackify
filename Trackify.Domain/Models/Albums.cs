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
        public int artistID { get; set; }
        public string albumImage { get; set; }
        public bool madePrivate { get; set; }
        public string color { get; set; }

        public Albums(int albumId, string albumTitle, AlbumType albumType, int artistID, string albumImage, bool madePrivate, string color)
        {
            this.albumId = albumId;
            this.albumTitle = albumTitle;
            this.albumType = albumType;
            this.artistID = artistID;
            this.albumImage = albumImage;
            this.madePrivate = madePrivate;
            this.color = color;
        }
    }
}
