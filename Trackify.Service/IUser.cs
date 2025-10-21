using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models;

namespace Trackify.Service
{
    public interface IUser
    {
        public Users SignUp(string username, string password, string confirmPassword, string firstName, string lastName, string email, DateOnly birthday, int subscriptionType);
        public int Login(string username, string password);
        public Users GetUserByID(int id);
        public Users UpdateUser(Users updatedUser);
        public void UpdatePassword(int id, string password);
    }
}
