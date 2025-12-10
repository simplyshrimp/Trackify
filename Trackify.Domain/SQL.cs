
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
        /*------------------------------------------------------------Admin---------------------------------------------------*/
        public void CreateAdmin()
        {
            using (SqlConnection conn  = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreateAdminUserSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
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
                        return new Users(userId, username, username, firstName, lastName, email, birthday, subscriptionType, pfp, accountAge, "#121212", false);
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
                                DateOnly.FromDateTime(reader.GetDateTime("AccountAge")),
                                reader.GetString("Color"),
                                reader.GetBoolean("IsAdmin")
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
                    cmd.Parameters.AddWithValue("@FirstName", updatedUser.firstName);
                    cmd.Parameters.AddWithValue("@LastName", updatedUser.lastName);
                    cmd.Parameters.AddWithValue("@Email", updatedUser.email);
                    cmd.Parameters.AddWithValue("@Birthday", updatedUser.birthday);
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
        public void UpdateProfile(Users user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateProfileSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", user.userId);
                    cmd.Parameters.AddWithValue("@Nickname", user.nickname);
                    cmd.Parameters.AddWithValue("@pfp", user.pfp);
                    cmd.Parameters.AddWithValue("@Color", user.color);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
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
        public List<Users> GetAllUsers()
        {
            List<Users> allUsers = new List<Users>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAllUsersSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            allUsers.Add(new Users(
                                reader.GetInt32("UserID"),
                                reader.GetString("Username"),
                                reader.GetString("Nickname"),
                                reader.GetString("FirstName"),
                                reader.GetString("LastName"),
                                reader.GetString("Email"),
                                DateOnly.FromDateTime(reader.GetDateTime("Birthday")),
                                (SubscriptionType)reader.GetInt32("SubscriptionType"),
                                reader.GetString("pfp"),
                                DateOnly.FromDateTime(reader.GetDateTime("AccountAge")),
                                reader.GetString("Color"),
                                reader.GetBoolean("IsAdmin")
                                ));
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            if (allUsers.Count > 0)
            {
                return allUsers;
            }
            else return null;
        }
        /*------------------------------------------Applications--------------------------------------------------*/

        public bool CreateApplication(int id)
        {
            try
            {
                 using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreateApplicationSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var work = cmd.ExecuteScalar();
                    if (work != null )
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        public bool DeleteApplication(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DeleteApplicationSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var work = cmd.ExecuteScalar();
                    if (work != null )
                       { return true; }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }
        public List<Applications> ShowAllApplications()
        {
            List<Applications> list = new List<Applications>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowAllApplicationsSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Applications(
                            reader.GetInt32("ApplicationID"),
                            GetUserByID(reader.GetInt32("UserID")),
                            DateOnly.FromDateTime(reader.GetDateTime("ApplicationDate")),
                            (AStatus)reader.GetInt32("ApplicationStatus")
                            ));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }
        public Applications ShowApplicationByID(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowApplicationByIDSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Applications(
                            reader.GetInt32("ApplicationID"),
                            GetUserByID(reader.GetInt32("UserID")),
                            DateOnly.FromDateTime(reader.GetDateTime("ApplicationDate")),
                            (AStatus)reader.GetInt32("ApplicationStatus")
                            );
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
        public void ChangeApplicationStatus(int userId, int newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ChangeApplicationStatusSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /*------------------------------------------Artist--------------------------------------------------*/
        public int CreateArtist(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreateArtistSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Artists ShowArtistByUserID(int UserID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowArtistByUserIDSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Artists(
                            reader.GetInt32("ArtistID"),
                            GetUserByID(UserID),
                            reader.GetBoolean("Verification")
                        );
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
        public void VerifyArtist(int artistId, bool verification)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("VerifyArtistSP", conn);
                    cmd.Parameters.AddWithValue("@ArtistID", artistId);
                    cmd.Parameters.AddWithValue("@Verification", verification);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Artists ShowArtistByID(int artistID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowArtistByIDSP", conn);
                    cmd.Parameters.AddWithValue("@ArtistID", artistID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Artists(
                            reader.GetInt32("ArtistID"),
                            GetUserByID(reader.GetInt32("UserID")),
                            reader.GetBoolean("Verification")
                            );
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
        public List<Artists> ShowAllArtists()
        {
            try
            {
                List<Artists> allArtists = new List<Artists>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowAllArtistSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                         allArtists.Add(new Artists(
                            reader.GetInt32("ArtistID"),
                            GetUserByID(reader.GetInt32("UserID")),
                            reader.GetBoolean("Verification")
                            ));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        /*------------------------------------------Album--------------------------------------------------*/
        public int CreateAlbum(int artistID, string albumTitle, AlbumType albumType, string color)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreateAlbumSP", conn);
                    cmd.Parameters.AddWithValue("@Artist", artistID);
                    cmd.Parameters.AddWithValue("@AlbumTitle", albumTitle);
                    cmd.Parameters.AddWithValue("@AlbumType", albumType);
                    cmd.Parameters.AddWithValue("@Color", color);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int userId = Convert.ToInt32(cmd.ExecuteScalar());
                    return userId;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void UpdateAlbum(Albums updatedAlbum)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateAlbumSP", conn);
                    cmd.Parameters.AddWithValue("@AlbumID", updatedAlbum.albumId);
                    cmd.Parameters.AddWithValue("@AlbumTitle", updatedAlbum.albumTitle);
                    cmd.Parameters.AddWithValue("@AlbumType", updatedAlbum.albumType);
                    cmd.Parameters.AddWithValue("@Artist", updatedAlbum.artist.artistID);
                    cmd.Parameters.AddWithValue("@AlbumImage", updatedAlbum.albumImage);
                    cmd.Parameters.AddWithValue("@MadePrivate", updatedAlbum.madePrivate);
                    cmd.Parameters.AddWithValue("@Color", updatedAlbum.color);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Albums GetAlbumByID(int albumID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAlbumByIDSP", conn);
                    cmd.Parameters.AddWithValue("@AlbumID", albumID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Albums(
                           reader.GetInt32("AlbumID"),
                           reader.GetString("AlbumTitle"),
                           (AlbumType)reader.GetInt32("AlbumType"),
                           ShowArtistByID(reader.GetInt32("Artist")),
                           reader.GetString("AlbumImage"),
                           reader.GetBoolean("MadePrivate"),
                           reader.GetString("Color"),
                           []
                           );
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public List<Albums> ShowAllAlbums()
        {
            try
            {
                List<Albums> allAlbums = new List<Albums>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowAllAlbumsSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                         allAlbums.Add(new Albums(
                           reader.GetInt32("AlbumID"),
                           reader.GetString("AlbumTitle"),
                           (AlbumType)reader.GetInt32("AlbumType"),
                           ShowArtistByID(reader.GetInt32("Artist")),
                           reader.GetString("AlbumImage"),
                           reader.GetBoolean("MadePrivate"),
                           reader.GetString("Color"),
                           []
                           ));
                    }
                }
                return allAlbums;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public List<Albums> ShowAllAlbumsByArtist(int artistID)
        {
            try
            {
                List<Albums> allAlbums = new List<Albums>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("ShowAllAlbumsSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allAlbums.Add(new Albums(
                          reader.GetInt32("AlbumID"),
                          reader.GetString("AlbumTitle"),
                          (AlbumType)reader.GetInt32("AlbumType"),
                          ShowArtistByID(reader.GetInt32("Artist")),
                          reader.GetString("AlbumImage"),
                          reader.GetBoolean("MadePrivate"),
                          reader.GetString("Color"),
                          []
                          ));
                    }
                }
                return allAlbums;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        /*------------------------------------------Songs--------------------------------------------------*/
        /*------------------------------------------Playlist--------------------------------------------------*/
        /*------------------------------------------Genre--------------------------------------------------*/
        /*------------------------------------------Subscriptions--------------------------------------------------*/
    }
}
