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
        private readonly IUser userMethod;
        public AddAlbumModel(IMusic music, IUser user)
        {
            musicMethod = music;
            userMethod = user;
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
                string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot/ImagesAndSongs/Albums/{id.ToString()}{Path.GetExtension(AlbumCover.FileName)}";
                using var filestream = new FileStream(filePath, FileMode.Create);
                AlbumCover.CopyTo(filestream);

                imagePath = $"/ImagesAndSongs/Albums/{id}{Path.GetExtension(AlbumCover.FileName)}";

                Albums updatedAlbum = new Albums{albumId = id, albumTitle = Title, albumType = AlbumTypeInput, artist = userMethod.ShowArtistByID((int)HttpContext.Session.GetInt32("ArtistID")), albumImage = imagePath, madePrivate = true, color = Color, songs= [] };
                musicMethod.UpdateAlbum(updatedAlbum);
                return RedirectToAlbumPage(id);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public IActionResult RedirectToAlbumPage(int albumId)
        {
            return Redirect($"/Artist/Album/{albumId}");
        }
    }
}
