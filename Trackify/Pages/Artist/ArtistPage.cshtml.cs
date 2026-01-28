using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.Artist
{
    public class ArtistPageModel : PageModel
    {
        private readonly IUser userMethod;
        private readonly IMusic musicMethod;
        public ArtistPageModel(IUser user,IMusic music)
        {
            userMethod = user;
            musicMethod = music;
        }

        [BindProperty(SupportsGet = true)]
        public int ArtistId { get; set; }
        public Artists Artist {  get; set; }
        public Users user = new Users();
        public List<Albums> ArtistAlbums {  get; set; }

        //for EditProfile
        [BindProperty]
        public IFormFile NewPfp { get; set; }

        [BindProperty]
        public string NewColor { get; set; }

        [BindProperty]
        public string NewNickname { get; set; }


        public void OnGet()
        {
            if (ArtistId > 0)
            {
                Artist = userMethod.ShowArtistByID(ArtistId);
                user = Artist.user;
                ArtistAlbums = musicMethod.ShowAllAlbumsByArtist(ArtistId);
                //remove artist constructor, remove it from the rest too while you're at it
            }
        }
        public IActionResult OnPostEditProfile()
        {

            user = userMethod.GetUserByID((int)HttpContext.Session.GetInt32("Id"));

            if (NewPfp != null)
            {
                if (user.pfp != null && user.pfp != "/ImagesAndSongs/Users/Empty-User-pfp.png")
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}\\wwwroot{user.pfp}");
                    if (file.Exists)
                    { file.Delete(); }
                }

                string imagePath = $"/ImagesAndSongs/Users/{user.userId}{Guid.NewGuid().ToString()}{Path.GetExtension(NewPfp.FileName)}";

                string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/{imagePath}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                NewPfp.CopyTo(filestream);

                user.pfp = imagePath;

                HttpContext.Session.SetString("pfp", imagePath);
            }

            if (NewColor != null)
            {
                user.color = NewColor;
            }

            if (NewNickname != null)
            {
                user.nickname = NewNickname;
            }

            userMethod.UpdateProfile(user);
            return Redirect($"/User/Profile/{user.userId}");
        }
    }
}
