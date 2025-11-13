using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.User
{
    public class AccountModel : PageModel
    {
        private readonly IUser userMethod;
        public AccountModel(IUser user)
        {
            userMethod = user;
        }
        [BindProperty]
        public Users? user { get; set; }
        public Applications? application { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                user = userMethod.GetUserByID((int)HttpContext.Session.GetInt32("Id"));
                application = userMethod.ShowApplicationByID((int)HttpContext.Session.GetInt32("Id"));
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
        public void OnPostApplication()
        {
            userMethod.CreateApplication((int)HttpContext.Session.GetInt32("Id")); 
        }
    }
}
