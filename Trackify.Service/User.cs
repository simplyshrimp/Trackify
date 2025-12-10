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

        public void CreateAdmin() => connection.CreateAdmin();
        public Users SignUp(string username, string password, string confirmPassword, string firstName, string lastName, string email, DateOnly birthday, SubscriptionType subscriptionType) => connection.SignUp(username, password, confirmPassword, firstName, lastName, email, birthday, subscriptionType);
        public int LoginByUsername(string username, string password) => connection.LoginByUsername(username, password);
        public int LoginByEmail(string email, string password) => connection.LoginByEmail(email, password);
        public Users GetUserByID(int id) => connection.GetUserByID(id);
        public bool GetUserByUsername(string username) => connection.GetUserByUsername(username);
        public bool GetUserByEmail(string email) => connection.GetUserByEmail(email);
        public Users UpdateUser(Users updatedUser) => connection.UpdateUser(updatedUser);
        public void UpdateProfile(Users user) => connection.UpdateProfile(user);
        public void UpdatePassword(int id, string newPassword) => connection.UpdatePassword(id, newPassword);
        public List<Users> GetAllUsers() => connection.GetAllUsers();

        /*-----------------------------------------------------*/
        public bool CreateApplication(int id) => connection.CreateApplication(id);
        public bool DeleteApplication(int id) => connection.DeleteApplication(id);
        public List<Applications> ShowAllApplications() => connection.ShowAllApplications();
        public Applications ShowApplicationByID(int id) => connection.ShowApplicationByID(id);
        public void ChangeApplicationStatus(int id, int status) => connection.ChangeApplicationStatus(id, status);

        /*-----------------------------------------------------*/
        public int CreateArtist(int userId) => connection.CreateArtist(userId);
        public Artists ShowArtistByUserID(int userId) => connection.ShowArtistByUserID(userId);
        public Artists ShowArtistByID(int artistId) => connection.ShowArtistByID(artistId);
        public void VerifyArtist(int artistId,bool verification) => connection.VerifyArtist(artistId, verification);
    }
}
