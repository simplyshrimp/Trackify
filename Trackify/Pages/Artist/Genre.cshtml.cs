using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages.Artist
{
    public class GenreModel : PageModel
    {
        private readonly IMusic musicMethod;
        private readonly IUser usermethod;
        public GenreModel(IMusic music, IUser user)
        {
            musicMethod = music;
            usermethod = user;
        }

        [BindProperty(SupportsGet = true)]
        public int GenreID { get; set; }
        public Genres genre { get; set; }
        public List<Songs>? songs { get; set; }

        public List<Playlists>? allUsersPlaylists = new List<Playlists>();
        public int Song {  get; set; }
        public int? PlaylistId { get; set; }

        public void OnGet()
        {
            genre = musicMethod.GetGenreByID(GenreID);
            songs = musicMethod.GetSongsByGenre(GenreID);
        }

        public void fillUserPlaylists()
        {
            musicMethod.GetAllUserPlaylists((int)HttpContext.Session.GetInt32("Id"));
        }
        public IActionResult OnPostSongPress()
        {
            string queue = musicMethod.MakeQueueStart(Song);

            Songs pressedSong = musicMethod.GetSongByID(Song);
            HttpContext.Session.SetString("Queue", queue);
            return Redirect($"/Artist/Genre/{GenreID}");
        }
        public IActionResult OnPostAddToQueue()
        {
            Songs pressedSong = musicMethod.GetSongByID(Song);
            string queue = musicMethod.AddToQueue(HttpContext.Session.GetString("Queue"), Song);
            HttpContext.Session.SetString("Queue", queue);

            return Redirect($"/Artist/Genre/{GenreID}");
        }
        public Artists GetSongArtist(Songs song)
        {
            return usermethod.ShowArtistByID(song.artistId);
        }
    }
}
