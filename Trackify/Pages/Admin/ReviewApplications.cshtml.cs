using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;
using Trackify.Service;

namespace Trackify.Pages.Admin
{
    public class ReviewApplicationsModel : PageModel
    {
        private readonly IUser userMethod;
        public ReviewApplicationsModel(IUser user)
        {
            userMethod = user;
        }
        public List<Applications> allApplications { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("Admin") == 1)
            {
                allApplications = userMethod.ShowAllApplications();
                return Page();
            }
            else
            {
                return RedirectToPage("/index");
            }
        }
        public string ChangeColorByStatus(AStatus currentStatus) //yes i know i have this somewhere else but i'm tired okay
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
