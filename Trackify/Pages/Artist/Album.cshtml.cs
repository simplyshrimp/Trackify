using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.Artist
{
    public class AlbumModel : PageModel
    {
        private readonly IMusic musicMethod;
        public AlbumModel(IMusic music)
        {
            musicMethod = music;
        }
        public Albums album {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public void OnGet()
        {
            album = musicMethod.GetAlbumByID(id);
        }
    }
}
