using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Service;
using Trackify.Domain.Models;
using System.Xml;

namespace Trackify.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUser userMethod;
        private readonly IMusic musicMethod;

        public IndexModel(ILogger<IndexModel> logger, IUser user, IMusic music)
        {
            _logger = logger;
            userMethod = user;
            musicMethod = music;
        }
        public List<Users>? allUsers = new List<Users>();
        public List<Albums>? allAlbums = new List<Albums>();
        public List<Artists>? allArtists = new List<Artists>();
        public void OnGetAsync()
        {
            if(HttpContext.Session.GetInt32("LoggedIn") != 1)
            {
                allUsers = userMethod.GetAllUsers();
                if (userMethod.GetAllUsers() == null)
                {
                    userMethod.CreateAdmin();
                }
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
}
