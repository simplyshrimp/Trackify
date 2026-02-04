using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages.Artist
{
    public class GenreModel : PageModel
    {
        private readonly IMusic musicMethod;
        public GenreModel(IMusic music)
        {
            musicMethod = music;
        }

        [BindProperty(SupportsGet = true)]
        public int GenreID { get; set; }
        public Genres genre { get; set; }
        public List<Songs>? songs { get; set; }

        public void OnGet()
        {
            genre = musicMethod.GetGenreByID(GenreID);
            songs = musicMethod.GetSongsByGenre(GenreID);
        }
    }
}
