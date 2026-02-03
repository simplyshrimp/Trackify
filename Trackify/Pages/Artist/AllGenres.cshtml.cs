using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages.Artist
{
    public class AllGenresModel : PageModel
    {
        private readonly IMusic musicMethod;
        public AllGenresModel(IMusic music)
        {
            musicMethod = music;
        }
        public List<Genres> allGenres = new();
        public void OnGet()
        {
            allGenres = musicMethod.GetAllGenres();
        }
    }
}
