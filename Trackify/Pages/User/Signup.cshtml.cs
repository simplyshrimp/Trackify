using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages.User
{
    public class SignupModel : PageModel
    {
        private readonly IUser userMethod;
        public SignupModel(IUser user)
        {
            userMethod = user;
        }
        [Required(ErrorMessage = "You must enter your username")]
        [BindProperty]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter a your email")]
        [BindProperty]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter your first name")]
        [BindProperty]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "enter your lastname")]
        [BindProperty]
        public string LastName { get; set; }
        [Required(ErrorMessage = "please enter your birthdate")]
        [BindProperty]
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "Please fill out your password")]
        [BindProperty]
        public string Password { get; set; }
        [Required(ErrorMessage = "you must confirm your password")]
        [BindProperty]
        public string ConfirmPassword { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid == true)
            {
                Users signedUp = userMethod.SignUp(Username, Password, ConfirmPassword, FirstName, LastName, Email, Birthday, 0);
                if (signedUp == null)
                {
                    ModelState.AddModelError(nameof(Email), "User could not be signed up");
                }
                else
                {
                    HttpContext.Session.SetInt32("LoggedIn", 1);
                    HttpContext.Session.SetInt32("Id", signedUp.userId);
                    HttpContext.Session.SetString("pfp", "/ImagesAndSongs/Users/Empty-User-pfp.png");
                    return RedirectToPage("/Homepage");
                }
            }
            else { }
            return Page();
        }
    }
}
