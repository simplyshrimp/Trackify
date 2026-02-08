using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages.Admin
{
    public class AddGenresModel : PageModel
    {
        private readonly IMusic musicMethod;
        public AddGenresModel(IMusic music)
        {
            musicMethod = music;
        }
        public List<Genres>? AllGenres = new List<Genres>();
        [BindProperty]
        public IFormFile? GenreFile { get; set; }
        [BindProperty]
        public string? GenreText { get; set; }
        [BindProperty]
        public int? GenreID { get; set; }
        public void OnGet()
        {
            AllGenres = musicMethod.GetAllGenres();
        }

        public IActionResult OnPost()
        {
            if (GenreFile != null)
            {
                using (var stream = GenreFile.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string fullFile = reader.ReadToEnd();
                        string[] splitFile = fullFile.Split(',');
                        foreach (string genre in splitFile)
                        {
                            musicMethod.CreateGenre(genre);
                        }
                    }
                }
            }
            if (GenreText != null)
            {
                string[] splitText = GenreText.Split(",");
                foreach (string genre in splitText)
                {
                    musicMethod.CreateGenre(genre);
                }
            }
            return RedirectToPage("/Admin/AddGenres");
        }
        public  IActionResult DeleteGenre(int GenreID)
        {
            musicMethod.DeleteGenreByID(GenreID);
            return RedirectToPage("/Admin/AddGenres");
        }
    }
}
