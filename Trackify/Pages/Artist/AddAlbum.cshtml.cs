using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Domain.Models.Enums;
using Trackify.Service;

namespace Trackify.Pages.Artist
{
    public class AddAlbumModel : PageModel
    {
        private readonly IMusic musicMethod;
        public AddAlbumModel(IMusic music)
        {
            musicMethod = music;
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
                int id = musicMethod.CreateAlbum((int)HttpContext.Session.GetInt32("ArtistID"), Title, AlbumTypeInput, Color);
                //if artist get ArtistID as session?

                string filePath = $"{Directory.GetCurrentDirectory()}/ImagesAndSongs/Albums/{id}{Path.GetExtension(AlbumCover.FileName)}";
                using var filestream = new FileStream(filePath, FileMode.Create);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
