using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;
using Trackify.Service;
using static System.Net.WebRequestMethods;

namespace Trackify.Pages.User
{
    public class AccountModel : PageModel
    {
        private readonly IUser userMethod;
        public AccountModel(IUser user)
        {
            userMethod = user;
        }
        
        public Users? user { get; set; }
        
        public Applications? application { get; set; }
        public Artists? artist {  get; set; } 
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                user = userMethod.GetUserByID((int)HttpContext.Session.GetInt32("Id"));
                application = userMethod.ShowApplicationByID(user.userId);
                artist = userMethod.ShowArtistByUserID(user.userId);
                return Page();
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
        public IActionResult OnPostApplication()
        {

            if (application == null) 
            {
                userMethod.CreateApplication((int)HttpContext.Session.GetInt32("Id"));
            }
            return RedirectToPage("/User/Account");
        }
        public IActionResult OnPostCancelApplication()
        {
            application = userMethod.ShowApplicationByID((int)HttpContext.Session.GetInt32("Id"));
            userMethod.ChangeApplicationStatus(application.User.userId, 4);
            return RedirectToPage("/User/Account");
        }
        public IActionResult OnPostAcceptArtist()
        {
            //should check if profile pic and not allow if not, but later
            int id = (int)HttpContext.Session.GetInt32("Id");
            artist = userMethod.ShowArtistByUserID(userMethod.CreateArtist(id));
            HttpContext.Session.SetInt32("ArtistID", artist.artistID);
            userMethod.ChangeApplicationStatus(id, 5);
            return RedirectToPage("/User/Account");

        }
        public IActionResult OnPostVerify()
        {
            artist = userMethod.ShowArtistByID((int)HttpContext.Session.GetInt32("ArtistID"));
            userMethod.VerifyArtist(artist.artistID,true);
            return RedirectToPage("/User/Account");
        }

        public string ChangeColorByStatus(AStatus currentStatus)
        {
            switch (currentStatus)
            {
                case AStatus.Pending:
                    {
                        return "#E6B81C";
                    }
                case AStatus.Accepted:
                    {
                        return "#7DD615";
                    }
                case AStatus.Denied:
                    {
                        return "#D93B14";
                    }
                case AStatus.Timed_out:
                    {
                        return "#C2BABA";
                    }
                case AStatus.Cancelled:
                    {
                        return "#6C728A";
                    }
                default:
                    {
                        return "#fffff";
                    }
            }
        }
    }
}
