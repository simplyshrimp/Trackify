using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;

namespace Trackify.Service
{
    public interface IMusic
    {
        public int CreateAlbum(int artistID, string albumtitle, AlbumType albumType, string color);
        public void UpdateAlbum(Albums updatedAlbum);
        public Albums GetAlbumByID(int albumID);
        public List<Albums> ShowAllAlbums();
        public List<Albums> ShowAllAlbumsByArtist(int artistID);
    }
}
