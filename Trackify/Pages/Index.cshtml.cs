using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Service;
using Trackify.Domain.Models;

namespace Trackify.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUser userMethod;

        public IndexModel(ILogger<IndexModel> logger, IUser user)
        {
            _logger = logger;
            userMethod = user;
        }
        public List<Users> allUsers { get; set; }
        public void OnGet()
        {
            if(HttpContext.Session.GetInt32("LoggedIn") != 1)
            {
                if (userMethod.GetAllUsers() == null)
                {
                    userMethod.CreateAdmin();
                }
            }
        }
    }
}
