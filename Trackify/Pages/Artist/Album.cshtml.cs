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

        //UploadSong

        [BindProperty]
        public IFormFile? NewSong { get; set; }

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
        }

        public void OnPostUploadSong()
        {
            string useString = $" (SPOTISAVER){Path.GetExtension(NewSong.FileName)}";

            Console.WriteLine(NewSong.Name);
            Console.WriteLine($"{NewSong.FileName.Replace($" (SPOTISAVER){Path.GetExtension(NewSong.FileName)}","")}");
            Console.WriteLine(NewSong.Length);
            /*string soundPath = $"/ImagesAndSongs/Users/{Guid.NewGuid().ToString()}{Path.GetExtension(NewSong.FileName)}";

            string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/{soundPath}";
            using var filestream = new FileStream(filePath, FileMode.Create);
            NewCover.CopyTo(filestream);

            //album.albumImage = soundPath;*/
            

            //HttpContext.Session.SetString("pfp", soundPath);
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

                HttpContext.Session.SetString("pfp", imagePath);
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
