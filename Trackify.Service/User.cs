using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain;
using Trackify.Domain.Models;
using Microsoft.Extensions.Configuration;
using Trackify.Domain.Models.Enums;

namespace Trackify.Service
{
    public class User : IUser
    {
        SQL connection;
        public User(IConfiguration configuration) => connection = new SQL(configuration);

        public Users SignUp(string username, string password, string confirmPassword, string firstName, string lastName, string email, DateOnly birthday, SubscriptionType subscriptionType) => connection.SignUp(username, password, confirmPassword, firstName, lastName, email, birthday, subscriptionType);
        public int LoginByUsername(string username, string password) => connection.LoginByUsername(username, password);
        public int LoginByEmail(string email, string password) => connection.LoginByEmail(email, password);
        public Users GetUserByID(int id) => connection.GetUserByID(id);
        public bool GetUserByUsername(string username) => connection.GetUserByUsername(username);
        public bool GetUserByEmail(string email) => connection.GetUserByEmail(email);
        public Users UpdateUser(Users updatedUser) => connection.UpdateUser(updatedUser);
        public void UpdatePassword(int id, string newPassword) => connection.UpdatePassword(id, newPassword);
    }
}
