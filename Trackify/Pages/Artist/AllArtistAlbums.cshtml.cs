using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages.Artist
{
    public class AllArtistAlbumsModel : PageModel
    {
        private readonly IMusic musicMethod;
        public AllArtistAlbumsModel(IMusic music)
        {
            musicMethod = music;
        }
        public List<Albums> allArtistAlbums { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (HttpContext.Session.GetInt32("ArtistID") != 0)
                {
                    allArtistAlbums = musicMethod.ShowAllAlbumsByArtist((int)HttpContext.Session.GetInt32("ArtistID"));
                    return Page();
                }
                else
                {
                    return RedirectToPage("/Homepage");
                }
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
