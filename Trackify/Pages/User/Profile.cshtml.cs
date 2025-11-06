using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        [BindProperty]
        public Users user { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") ==1)
            {
                if (HttpContext.Session.GetInt32("Id") != 0)
                {
                    user = userMethod.GetUserByID((int)HttpContext.Session.GetInt32("Id"));
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
    }
}
