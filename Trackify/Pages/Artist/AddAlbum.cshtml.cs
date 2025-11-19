using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Domain.Models.Enums;
using Trackify.Service;

namespace Trackify.Pages.Artist
{
    public class AddAlbumModel : PageModel
    {
        private readonly IUser userMethod;
        public AddAlbumModel(IUser user)
        {
            userMethod = user;
        }
        [Required(ErrorMessage = "You need to add a title to your album")]
        [BindProperty]
        public string Title { get; set; }
        [Required(ErrorMessage = "You need to choose an albumtype")]
        [BindProperty]
        public AlbumType AlbumTypeInput { get; set; }
        [BindProperty]
        public string? Color { get; set; }
        [Required(ErrorMessage = "You need to upload an album cover")]
        [BindProperty]
        public IFormFile AlbumCover { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            try
            {
                string imagePath = "";
                //int id = musicMethod          do sql first, either update whole album and make 1 or update just image and dod 2

                string filePath = $"{Directory.GetCurrentDirectory()}/ImagesAndSongs/Albums/{id}{Path.GetExtension(AlbumCover)}";
                using var filestream = new FileStream(filePath, FileMode.Create);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
