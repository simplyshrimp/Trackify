using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Trackify.Pages.Artist
{
    public class GenreModel : PageModel
    {


        [BindProperty(SupportsGet = true)]
        public int GenreID { get; set; }


        public void OnGet()
        {
        }
    }
}
