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
                //remove artist constructor, remove it from the rest too while you're at it
            }
        }
        public IActionResult OnPostEditProfile()
        {

            user = userMethod.GetUserByID((int)HttpContext.Session.GetInt32("Id"));

            if (NewPfp != null)
            {
                string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/ImagesAndSongs/Users/{user.userId}{Path.GetExtension(NewPfp.FileName)}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                NewPfp.CopyTo(filestream);

                string imagePath = $"\\ImagesAndSongs\\Users\\{user.userId}{Path.GetExtension(NewPfp.FileName)}";
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
