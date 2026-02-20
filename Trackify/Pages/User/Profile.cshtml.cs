using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.User
{
    public class ProfileModel : PageModel
    {
        private readonly IUser userMethod;
        private readonly IMusic musicMethod;
        public ProfileModel(IUser user, IMusic music)
        {
            userMethod = user;
            musicMethod = music;
        }

        //for displaying profile
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        //for EditProfile
        [BindProperty]
        public IFormFile NewPfp { get; set; }

        [BindProperty]
        public string NewColor { get; set; }

        [BindProperty]
        public string NewNickname { get; set; }

        public Users user = new Users();
        public List<Playlists> userAllPlaylists = new List<Playlists>();
        public List<Playlists> userPublicPlaylists = new List<Playlists>();

        //methods
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (Id != 0)
                {
                    user = userMethod.GetUserByID((int)Id);
                    userAllPlaylists = musicMethod.GetAllUserPlaylists(Id); //seperate public
                    foreach(Playlists playlist in userAllPlaylists)
                    {
                        if(playlist.MadePrivate == false)
                            userPublicPlaylists.Add(playlist);
                    }
                    return Page();
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else { return RedirectToPage("/Index"); }

            //make artist application now and then songs
        }

        public IActionResult OnPostEditProfile()
        {
            user = userMethod.GetUserByID((int)Id);

            if (NewPfp != null)
            {
                if (user.pfp != null && user.pfp != "/ImagesAndSongs/Users/Empty-User-pfp.png")
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}\\wwwroot{user.pfp}");
                    if (file.Exists)
                        { file.Delete(); }
                }

                string imagePath = $"/ImagesAndSongs/Users/{user.userId}{Guid.NewGuid().ToString()}{Path.GetExtension(NewPfp.FileName)}";

                string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot/{imagePath}";
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
