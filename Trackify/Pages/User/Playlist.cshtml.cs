using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.User
{
    public class PlaylistModel : PageModel
    {
        private readonly IMusic musicMethod;
        private readonly IUser userMethod;
        public PlaylistModel(IMusic music, IUser user)
        {
            musicMethod = music;
            userMethod = user;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Playlists? Playlist { get; set; }
        public Users? playlistUser { get; set; }
        public TimeSpan totalTime { get; set; }
        public string? PrivacyStatus { get; set; }

        //for EditPlaylist
        [BindProperty]
        public IFormFile? NewCover { get; set; }
        [BindProperty]
        public string? NewColor { get; set; }

        [BindProperty]
        public string? NewTitle { get; set; }
        [BindProperty]
        public bool? NewPrivacy { get; set; }

        //for playing the songs
        public int Song { get; set; }

        public IActionResult OnGet()
        {
            if (Id == 0)
            {
                return RedirectToPage("Index");
            }
            else
            {
                musicMethod.GetPlaylistByID(Id);
                playlistUser = userMethod.GetUserByID(Playlist.UserId);
                totalTime = musicMethod.GetTotalDuration(Id);
                PrivacyStatus = "";
                if (Playlist.MadePrivate)
                    PrivacyStatus = "private";
                else
                    PrivacyStatus = "public";
                return Page();
            }
            
        }

        public IActionResult OnPostSongPress()
        {
            string queue = musicMethod.MakeQueueStart(Song);

            Songs pressedSong = musicMethod.GetSongByID(Song);
            HttpContext.Session.SetString("Queue", queue);
            return Redirect($"/Artist/Album/{pressedSong.albumId}");
        }
        public IActionResult OnPostAddToQueue()
        {
            Songs pressedSong = musicMethod.GetSongByID(Song);
            string queue = musicMethod.AddToQueue(HttpContext.Session.GetString("Queue"), Song);
            HttpContext.Session.SetString("Queue", queue);

            return Redirect($"/Artist/Album/{pressedSong.albumId}");
        }

        public IActionResult OnPostEditPlaylist()
        {
            Playlist = musicMethod.GetPlaylistByID(Id);
            if (NewCover != null)
            {
                if (Playlist.PlaylistImage != null && Playlist.PlaylistImage != "/ImagesAndSongs/Playlists/Trackify-Album-Placeholder.png")
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}\\wwwroot{Playlist.PlaylistImage}");
                    if (file.Exists)
                    { file.Delete(); }
                }

                string imagePath = $"/ImagesAndSongs/Users/{Id}{Guid.NewGuid().ToString()}{Path.GetExtension(NewCover.FileName)}";

                string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot/{imagePath}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                NewCover.CopyTo(filestream);

                Playlist.PlaylistImage = imagePath;
            }

            if (NewColor != null)
            {
                Playlist.PlaylistsColor = NewColor;
            }

            if (NewTitle != null)
            {
                Playlist.PlaylistName = NewTitle;
            }

            if (NewPrivacy != null)
            {
                Playlist.MadePrivate = (bool)NewPrivacy;
            }

            musicMethod.EditPlaylist(Playlist);
            return Redirect($"/User/Playlist/{Playlist.PlaylistId}");
        }
    }
}