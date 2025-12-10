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
        public ProfileModel(IUser user)
        {
            userMethod = user;
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
        //methods
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                if (Id != 0)
                {
                    user = userMethod.GetUserByID((int)Id);
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
                string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/ImagesAndSongs/Users/{Id}{Path.GetExtension(NewPfp.FileName)}";
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

            Console.WriteLine(HttpContext.Session.GetString("pfp"));

            userMethod.UpdateProfile(user);
            return Redirect($"/User/Profile/{user.userId}");
        }
    }
}
