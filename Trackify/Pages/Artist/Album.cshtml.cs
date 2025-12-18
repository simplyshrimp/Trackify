using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;
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
        public Albums album {  get; set; }
        public Artists artist {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public TimeSpan totalTime { get; set; }

        //UploadSong

        [BindProperty]
        public IFormFile? NewSong { get; set; }
        [BindProperty]
        public int genre { get; set; }

        public List<Domain.Models.Genres> allGenres = new List<Domain.Models.Genres>();

        //for EditAlbum

        [BindProperty]
        public IFormFile? NewCover { get; set; }
        [BindProperty]
        public string? NewColor { get; set; }

        [BindProperty]
        public string? NewTitle { get; set; }
        public void OnGet()
        {
            album = musicMethod.GetAlbumByID(id);
            allGenres = musicMethod.GetAllGenres();
            totalTime = musicMethod.GetTotalDuration(id);
        }

        public IActionResult OnPostUploadSong()
        {
            Songs song = new Songs();
            album = musicMethod.GetAlbumByID(id);

            string songPath = $"/ImagesAndSongs/Songs/{Guid.NewGuid().ToString()}{Path.GetExtension(NewSong.FileName)}";
            string testFilePath = $"{Directory.GetCurrentDirectory()}/wwwroot{songPath}";
        
            string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/{songPath}";
            using var filestream = new FileStream(testFilePath, FileMode.Create);
            NewSong.CopyTo(filestream);
            filestream.Close();

            TagLib.File tagLibFile = TagLib.File.Create(filePath);

            song.length = tagLibFile.Properties.Duration;
            song.title = tagLibFile.Tag.Title;
            song.albumId = album.albumId;
            song.artistId = album.artist.artistID;
            song.filepath = songPath;
            song.albumTrackNr = (int)tagLibFile.Tag.Track;
            song.genre = musicMethod.GetGenreByID(genre);


            musicMethod.CreateSong(song);
            return Redirect($"/Artist/Album/{id}");
        }
        public IActionResult OnPostEditAlbum()
        {

            if (NewCover != null)
            {
                if (album.albumImage != null && album.albumImage != "/ImagesAndSongs/Users/Empty-User-pfp.png")
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}\\wwwroot{album.albumImage}");
                    if (file.Exists)
                    { file.Delete(); }
                }

                string imagePath = $"/ImagesAndSongs/Users/{album.albumId}{Guid.NewGuid().ToString()}{Path.GetExtension(NewCover.FileName)}";

                string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/{imagePath}";
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

            musicMethod.UpdateAlbum(album);
            return Redirect($"/User/Profile/{album.albumId}");
        }
    }
}
