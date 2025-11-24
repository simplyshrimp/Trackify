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
        [BindProperty(SupportsGet = true)]
        public Albums album {  get; set; }
        public int id { get; set; }
        public void OnGet()
        {
        }
    }
}
