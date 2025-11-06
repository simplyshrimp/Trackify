
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;

namespace Trackify.Domain
{
    public class SQL
    {
        private readonly string connectionString;
        public SQL(IConfiguration configuration) => connectionString = configuration.GetConnectionString("Default");

        /*------------------------------------------------------------Users---------------------------------------------------*/
        public Users SignUp(string username, string password, string confirmPassword, string firstName, string lastName, string email, DateOnly birthday, SubscriptionType subscriptionType)
        {
            if (password == confirmPassword)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("CreateUserSP", conn);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Birthday", birthday);
                        cmd.Parameters.AddWithValue("@Subscription", Convert.ToInt32(subscriptionType));
                        cmd.CommandType = CommandType.StoredProcedure;
                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        DateOnly accountAge = DateOnly.FromDateTime(DateTime.Now);
                        string pfp = "/ImagesAndSong/Users/empty-user-pfp.png";
                        return new Users(userId, username, username, firstName, lastName, email, birthday, subscriptionType, pfp, accountAge);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("something went wrong..");
                        throw;
                    }
                    finally { conn.Close(); }
                }
            }
            return null;
        }

        public int LoginByUsername(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LoginUsernameSP", conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int userId = Convert.ToInt32(cmd.ExecuteScalar());
                    return userId;
                }
                catch (Exception)
                {
                    Console.WriteLine("login went wrong");
                    throw;
                }
            }
        }
        public int LoginByEmail(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LoginEmailSP", conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int userId = Convert.ToInt32(cmd.ExecuteScalar());
                    return userId;
                }
                catch (Exception)
                {
                    Console.WriteLine("login went wrong");
                    throw;
                }
            }
        }

        public Users GetUserByID(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetUserByIDSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return new Users(
                                id,
                                reader.GetString("Username"),
                                reader.GetString("Nickname"),
                                reader.GetString("FirstName"),
                                reader.GetString("LastName"),
                                reader.GetString("Email"),
                                DateOnly.FromDateTime(reader.GetDateTime("Birthday")),
                                (SubscriptionType)reader.GetInt32("SubscriptionType"),
                                reader.GetString("pfp"),
                                DateOnly.FromDateTime(reader.GetDateTime("AccountAge"))
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;

        }
        public bool GetUserByUsername(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetUserByUsernameSP", conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }
        public bool GetUserByEmail(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetUserByEmailSP", conn);
                    cmd.Parameters.AddWithValue ("@Email", email);
                    cmd.CommandType= CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }


        public Users UpdateUser(Users updatedUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateUserSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", updatedUser.userId);
                    cmd.Parameters.AddWithValue("@Username", updatedUser.username);
                    cmd.Parameters.AddWithValue("@Nickname", updatedUser.nickname);
                    cmd.Parameters.AddWithValue("@FirstName", updatedUser.firstName);
                    cmd.Parameters.AddWithValue("@LastName", updatedUser.lastName);
                    cmd.Parameters.AddWithValue("@Email", updatedUser.email);
                    cmd.Parameters.AddWithValue("@Birthday", updatedUser.birthday);
                    cmd.Parameters.AddWithValue("@pfp", updatedUser.pfp);
                    cmd.Parameters.AddWithValue("@SubscriptionType", updatedUser.subscriptionType);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    return updatedUser;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdatePassword(int id, string newPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdatePasswordSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.Parameters.AddWithValue("Password", newPassword);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /*------------------------------------------Applications--------------------------------------------------*/

        public bool CreateApplication(int id)
        {
            try
            {
                 
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }
    }
}
