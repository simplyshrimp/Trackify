using Microsoft.VisualBasic;
using TagLib;

namespace Trackify.Pages.Components.FormData_Class
{
    public class CPlaylistModel
    {
        public string PlaylistName { get; set; }
        public IFormFile PlaylistImage { get; set; }
        public string PlaylistColor { get; set; }
        public bool MadePrivate { get; set; }
    }
}
