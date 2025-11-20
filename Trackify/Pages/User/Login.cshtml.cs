using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages.User
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
                    Users loggedIn = userMethod.GetUserByID(id);
                    HttpContext.Session.SetInt32("LoggedIn", 1);
                    HttpContext.Session.SetInt32("Id", id);
                    HttpContext.Session.SetString("pfp", loggedIn.pfp);
                    if (userMethod.ShowArtistByUserID(id) != null)
                    {
                        HttpContext.Session.SetInt32("ArtistID", userMethod.ShowArtistByUserID(id).artistID);
                    }
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
