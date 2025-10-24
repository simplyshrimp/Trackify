using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Service;

namespace Trackify.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUser userMethod;
        public LoginModel(IUser user)
        {
            userMethod = user;
        }
        [Required(ErrorMessage = "You must write either your username or your password")]
        [BindProperty]
        public string EmailOrUsername { get; set; }
        [Required(ErrorMessage = "Please fill out your password")]
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                int id = 0;
                if (EmailOrUsername.Contains('@') == true)
                {
                    id = userMethod.LoginByEmail(EmailOrUsername, Password);
                }
                else
                {
                    id = userMethod.LoginByUsername(EmailOrUsername, Password);
                }
                if (id == 0)
                {
                    ModelState.AddModelError(nameof(Password), "Your login was incorrect");
                }
                else
                {
                    HttpContext.Session.SetInt32("LoggedIn", 1);
                    HttpContext.Session.SetInt32("Id", id);
                    return RedirectToPage("/HomePage");
                }
            }
            else
            {

            }
            return Page();
        }
    }
}
