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
        public int? id { get; set; }
        [BindProperty]
        public Users user { get; set; }

        //for EditProfile
        [BindProperty]
        public IFormFile? NewPfp { get; set; }
        [BindProperty]
        public string? NewColor { get; set; }
        [BindProperty]
        public string? NewNickname { get; set; }
        //methods
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") ==1)
            {
                if (id != 0)
                {
                    user = userMethod.GetUserByID((int)id);
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
        public IActionResult OnDialogEditProfile()
        {
            id = HttpContext.Session.GetInt32("Id");
            Users newUser = userMethod.GetUserByID((int)id);

            if (NewPfp != null)
            {
                string imagePath = "";
                string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/ImagesAndSongs/Users/{id}{Path.GetExtension(NewPfp.FileName)}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                NewPfp.CopyTo(filestream);

                imagePath = $"/ImagesAndSongs/Albums/{id}{Path.GetExtension(NewPfp.FileName)}";
                newUser.pfp = imagePath;
            }
            if (NewColor != null)
            {
                newUser.color = NewColor;
            }
            if (NewNickname != null)
            {
                newUser.nickname = NewNickname;
            }
            userMethod.UpdateUser(newUser);
            return RedirectToPage($"/User/Profile/{newUser.userId}");
        }
    }
}
