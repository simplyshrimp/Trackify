using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Trackify.Domain.Models;
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
        [Required (ErrorMessage = "please choose a color for your album")]
        [BindProperty]
        public string Color { get; set; }
        [Required(ErrorMessage = "You need to upload an album cover")]
        [BindProperty]
        public IFormFile AlbumCover { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                string imagePath = "";
                int id = musicMethod.CreateAlbum((int)HttpContext.Session.GetInt32("ArtistID"), Title, AlbumTypeInput, Color);
            //if artist get ArtistID as session?
                string filePath = $"C:/Users/cecby0001/source/repos/Trackify/Trackify/wwwroot/ImagesAndSongs/Albums/{id}{Path.GetExtension(AlbumCover.FileName)}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                AlbumCover.CopyTo(filestream);

                imagePath = $"/ImagesAndSongs/Albums/{id}{Path.GetExtension(AlbumCover.FileName)}";

                Albums updatedAlbum = new(id, Title, AlbumTypeInput, (int)HttpContext.Session.GetInt32("ArtistID"), imagePath, true, Color);
                musicMethod.UpdateAlbum(updatedAlbum);
                return RedirectToPage($"/Artist/Album/{id}");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
