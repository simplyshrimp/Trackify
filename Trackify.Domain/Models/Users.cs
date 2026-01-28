using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models.Enums;

namespace Trackify.Domain.Models
{
    public class Users
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateOnly birthday { get; set; }
        public SubscriptionType subscriptionType { get; set; }
        public string pfp { get; set; }
        public DateOnly accountAge { get; set; }
        public string color { get; set; }
        public bool isAdmin { get; set; }
    }
}
