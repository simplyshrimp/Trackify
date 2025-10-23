using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Service;

namespace Trackify.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IUser userMethod;
        public SignupModel(IUser user)
        {
            userMethod = user;
        }
        [Required(ErrorMessage = "You must write either your username or your password")]
        [BindProperty]
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "Please fill out your password")]
        [BindProperty]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public void OnGet()
        {
        }
    }
}
