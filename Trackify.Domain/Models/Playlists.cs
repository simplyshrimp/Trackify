using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models.Enums;

namespace Trackify.Domain.Models
{
    public class Playlists
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistImage { get; set; }
        public bool MadePrivate { get; set; }
        public string PlaylistsColor { get; set; }
        public List<Songs> Songs { get; set; }
    }
}
