using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;

namespace Trackify.Service
{
    public interface IUser
    {
        public Users SignUp(string username, string password, string confirmPassword, string firstName, string lastName, string email, DateOnly birthday, SubscriptionType subscriptionType);
        public int LoginByUsername(string username, string password);
        public int LoginByEmail(string email, string password);
        public Users GetUserByID(int id);
        public bool GetUserByUsername(string username);
        public bool GetUserByEmail(string email);
        public Users UpdateUser(Users updatedUser);
        public void UpdatePassword(int id, string password);

        public bool CreateApplication(int id);
        public bool DeleteApplication(int id);
        public List<Applications> ShowAllApplications();
        public Applications ShowApplicationByID(int id);
        public void ChangeApplicationStatus(int id, int status);
    }
}
