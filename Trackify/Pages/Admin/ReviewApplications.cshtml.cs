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

        [BindProperty(SupportsGet = true)]
        public int? filterOptions { get; set; }

        public List<Applications> allApplications { get; set; }
        public List<Applications> filteredApplications { get; set; }
        public List<Applications> chosenList { get; set; }

        [BindProperty]
        public int hiddenID { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("Admin") == 1)
            {
                allApplications = userMethod.ShowAllApplications();
                chosenList = allApplications;
                if (filterOptions == null)
                {
                    chosenList = allApplications;
                    return Page();
                }
                else
                {
                    filteredApplications = new List<Applications>();
                    foreach (Applications apps in allApplications)
                    {
                        if ((AStatus)filterOptions == apps.Status)
                        {
                            filteredApplications.Add(apps);
                        }
                    }
                    chosenList = filteredApplications;
                    return Page();
                }
                
            }
            else
            {
                return RedirectToPage("/index");
            }
        }
        public IActionResult OnPostAccept()
        {
            userMethod.ChangeApplicationStatus(hiddenID, 1);
            return RedirectToPage("/Admin/ReviewApplications");
        }
        public IActionResult OnPostDeny()
        {
            userMethod.ChangeApplicationStatus(hiddenID, 2);
            return RedirectToPage("/Admin/ReviewApplications");
        }
        public IActionResult OnPostChange()
        {
            userMethod.ChangeApplicationStatus(hiddenID, 0);
            return RedirectToPage("/Admin/ReviewApplications");
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
