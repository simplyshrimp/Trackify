using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.Artist
{
    public class AlbumModel : PageModel
    {
        private readonly IMusic musicMethod;
        private readonly IUser userMethod;
        public AlbumModel(IMusic music, IUser user)
        {
            musicMethod = music;
            userMethod = user;
        }
        public Albums album {  get; set; }
        public Artists artist {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public void OnGet()
        {
            album = musicMethod.GetAlbumByID(id);
            artist = userMethod.ShowArtistByID(album.artistID);
        }
    }
}
