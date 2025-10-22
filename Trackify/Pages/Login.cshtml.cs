using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Trackify.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.SetInt32("NoNavBar", 1);
        }
    }
}
