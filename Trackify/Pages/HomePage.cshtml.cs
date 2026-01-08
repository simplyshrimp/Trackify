using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Service;

namespace Trackify.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly IUser userMethod;
        private readonly IMusic musicMethod;

        public HomePageModel(IUser user, IMusic music)
        {
            userMethod = user;
            musicMethod = music;
        }

        public List<Users>? allUsers = new List<Users>();
        public List<Albums>? allAlbums = new List<Albums>();
        public List<Artists>? allArtists = new List<Artists>();
        public void OnGet()
        {
            allUsers = userMethod.GetAllUsers();
            allAlbums = musicMethod.ShowAllAlbums();
            foreach (Users user in allUsers)
            {
                if (userMethod.ShowArtistByUserID(user.userId) != null)
                {
                    allArtists.Add(userMethod.ShowArtistByUserID(user.userId));
                }
            }
        }
    }
}
