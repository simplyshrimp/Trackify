using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;

namespace Trackify.Service
{
    public class Music : IMusic
    {
        SQL connection;
        public Music(IConfiguration configuration) => connection = new SQL(configuration);
        public int CreateAlbum(int artistID, string albumTitle, AlbumType albumType, string color) => connection.CreateAlbum(artistID, albumTitle, albumType, color);
        public void UpdateAlbum(Albums updatedAlbum) => connection.UpdateAlbum(updatedAlbum);
    }
}
