using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.User
{
    public class EditInfoModel : PageModel
    {
        private readonly IUser userMethod;
        public EditInfoModel(IUser user)
        {
            userMethod = user;
        }
        public Users user { get; set; }
        public void OnGet()
        {
        }
    }
}
