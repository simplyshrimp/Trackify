using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.Domain.Models
{
    public class Users
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateOnly birthday { get; set; }
        public int subscriptionType { get; set; }
        public string pfp { get; set; }
        public DateOnly accountAge { get; set; }


        public Users(int UserID, string username, string email, string firstName, string lastName, DateOnly birthday, int subscriptionType, string pfp, DateOnly accountAge)
        {
            this.userId = UserID;
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.birthday = birthday;
            this.subscriptionType = subscriptionType;
            this.pfp = pfp;
            this.accountAge = accountAge;   
        }
    }
}
