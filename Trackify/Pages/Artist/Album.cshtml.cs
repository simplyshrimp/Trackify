using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using TagLib;
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
        [BindProperty]
        public Albums album {  get; set; }
        public Artists artist {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public TimeSpan totalTime { get; set; }
        public string PrivacyStatus { get; set; }

        public List<Playlists>? allUsersPlaylists = new List<Playlists>();
        //UploadSong

        [BindProperty]
        public IFormFile? NewSong { get; set; }
        [BindProperty]
        public int? genre { get; set; }

        public List<Domain.Models.Genres>? allGenres = new List<Domain.Models.Genres>();

        //for EditAlbum

        [BindProperty]
        public IFormFile? NewCover { get; set; }
        [BindProperty]
        public string? NewColor { get; set; }

        [BindProperty]
        public string? NewTitle { get; set; }
        [BindProperty]
        public bool? NewPrivacy { get; set; }

        [BindProperty]
        public int Song { get; set; }

        [BindProperty]
        public int PlaylistId { get; set; }
        public void OnGetAsync()
        {
            album = musicMethod.GetAlbumByID(id);
            allGenres = musicMethod.GetAllGenres();
            totalTime = musicMethod.GetTotalDuration(id);
            PrivacyStatus = "";
            if (album.madePrivate)
                PrivacyStatus = "private";
            else
                PrivacyStatus = "public";
        }

        public IActionResult OnPostSongPress()
        {
            string queue = musicMethod.MakeQueueStart(Song);

            Songs pressedSong = musicMethod.GetSongByID(Song);
            /*Albums fromAlbum = musicMethod.GetAlbumByID(pressedSong.albumId);
            StringBuilder sb = new StringBuilder();
            sb.Append(pressedSong.songId.ToString());
            Console.WriteLine(fromAlbum.songs.IndexOf(pressedSong));
            foreach (Songs song in fromAlbum.songs)
            {
                Console.WriteLine(fromAlbum.songs.IndexOf(pressedSong));
                Console.WriteLine(fromAlbum.songs.IndexOf(song));
                if (fromAlbum.songs.IndexOf(song) > fromAlbum.songs.IndexOf(pressedSong))
                {

                    sb.Append(",");
                    sb.Append(song.songId);
                }
            }*/
            HttpContext.Session.SetString("Queue", queue);
            //Console.WriteLine(Song);
            return Redirect($"/Artist/Album/{pressedSong.albumId}");
        }
        public IActionResult OnPostAddToQueue()
        {
            Songs pressedSong = musicMethod.GetSongByID(Song);
            string queue = musicMethod.AddToQueue(HttpContext.Session.GetString("Queue"), Song);
            HttpContext.Session.SetString("Queue", queue);

            return Redirect($"/Artist/Album/{pressedSong.albumId}");
        }

        public IActionResult OnPostUploadSong()
        {
            Songs song = new Songs();
            album = musicMethod.GetAlbumByID(id);

            string songPath = $"/ImagesAndSongs/Songs/{Guid.NewGuid().ToString()}{Path.GetExtension(NewSong.FileName)}";
            string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot{songPath}";
        
            using var filestream = new FileStream(filePath, FileMode.Create);
            NewSong.CopyTo(filestream);
            filestream.Close();

            TagLib.File tagLibFile = TagLib.File.Create(filePath);

            song.length = tagLibFile.Properties.Duration;
            song.title = tagLibFile.Tag.Title;
            song.albumId = album.albumId;
            song.artistId = album.artist.artistID;
            song.filepath = songPath;
            song.albumTrackNr = (int)tagLibFile.Tag.Track;
            song.genre = musicMethod.GetGenreByID((int)genre);


            musicMethod.CreateSong(song);
            return Redirect($"/Artist/Album/{id}");
        }
        public IActionResult OnPostEditAlbum()
        {
            album = musicMethod.GetAlbumByID(id);
            if (NewCover != null)
            {
                if (album.albumImage != null && album.albumImage != "/ImagesAndSongs/Albums/Trackify-Song-Placeholder.png")
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}\\wwwroot{album.albumImage}");
                    if (file.Exists)
                    { file.Delete(); }
                }

                string imagePath = $"/ImagesAndSongs/Album/{album.albumId}{Guid.NewGuid().ToString()}{Path.GetExtension(NewCover.FileName)}";

                string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot/{imagePath}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                NewCover.CopyTo(filestream);

                album.albumImage = imagePath;
            }

            if (NewColor != null)
            {
                album.color = NewColor;
            }

            if (NewTitle != null)
            {
                album.albumTitle = NewTitle;
            }

            if (NewPrivacy != null)
            {
                album.madePrivate = (bool)NewPrivacy;
            }

            musicMethod.UpdateAlbum(album);
            return Redirect($"/Artist/Album/{album.albumId}");
        }

        public List<Playlists>? GetUserPlaylists()
        {

            allUsersPlaylists = musicMethod.GetAllUserPlaylists((int)HttpContext.Session.GetInt32("Id"));
            return allUsersPlaylists;
        }
        public IActionResult OnPostAddSongToPlaylist()
        {
            musicMethod.AddSongToPlaylist(Song, PlaylistId);
            return Redirect($"/Artist/Album/{id}");
        }
    }
}
