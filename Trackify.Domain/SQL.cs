
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
                        return new Users{userId=userId, username=username, nickname=username, firstName=firstName, lastName = lastName, email = email, birthday = birthday, subscriptionType = subscriptionType, pfp = pfp, accountAge = accountAge, color = "#121212", isAdmin= false};
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
                            return new Users {
                                userId = id,
                                username = reader.GetString("Username"),
                                nickname = reader.GetString("Nickname"),
                                firstName = reader.GetString("FirstName"),
                                lastName = reader.GetString("LastName"),
                                email = reader.GetString("Email"),
                                birthday = DateOnly.FromDateTime(reader.GetDateTime("Birthday")),
                                subscriptionType = (SubscriptionType)reader.GetInt32("SubscriptionType"),
                                pfp = reader.GetString("pfp"),
                                accountAge = DateOnly.FromDateTime(reader.GetDateTime("AccountAge")),
                                color = reader.GetString("Color"),
                                isAdmin = reader.GetBoolean("IsAdmin")
                                };
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
                            allUsers.Add(new Users {
                                userId = reader.GetInt32("UserID"),
                                username = reader.GetString("Username"),
                                nickname = reader.GetString("Nickname"),
                                firstName = reader.GetString("FirstName"),
                                lastName = reader.GetString("LastName"),
                                email = reader.GetString("Email"),
                                birthday = DateOnly.FromDateTime(reader.GetDateTime("Birthday")),
                                subscriptionType = (SubscriptionType)reader.GetInt32("SubscriptionType"),
                                pfp = reader.GetString("pfp"),
                                accountAge = DateOnly.FromDateTime(reader.GetDateTime("AccountAge")),
                                color = reader.GetString("Color"),
                                isAdmin = reader.GetBoolean("IsAdmin")
                            });
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
                        list.Add(new Applications {
                            Id = reader.GetInt32("ApplicationID"),
                            User = GetUserByID(reader.GetInt32("UserID")),
                            ApplicationDate = DateOnly.FromDateTime(reader.GetDateTime("ApplicationDate")),
                            Status = (AStatus)reader.GetInt32("ApplicationStatus")
                        });
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
                        return new Applications {
                            Id = reader.GetInt32("ApplicationID"),
                            User = GetUserByID(reader.GetInt32("UserID")),
                            ApplicationDate = DateOnly.FromDateTime(reader.GetDateTime("ApplicationDate")),
                            Status = (AStatus)reader.GetInt32("ApplicationStatus")
                            };
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
                        return new Artists {
                            artistID = reader.GetInt32("ArtistID"),
                            user = GetUserByID(UserID),
                            verified = reader.GetBoolean("Verification")
                        };
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
                        return new Artists {
                            artistID = reader.GetInt32("ArtistID"),
                            user = GetUserByID(reader.GetInt32("UserID")),
                            verified = reader.GetBoolean("Verification")
                            };
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
                         allArtists.Add(new Artists {
                            artistID = reader.GetInt32("ArtistID"),
                            user = GetUserByID(reader.GetInt32("UserID")),
                            verified = reader.GetBoolean("Verification")
                            });
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
                        return new Albums {
                           albumId = reader.GetInt32("AlbumID"),
                           albumTitle = reader.GetString("AlbumTitle"),
                           albumType = (AlbumType)reader.GetInt32("AlbumType"),
                           artist = ShowArtistByID(reader.GetInt32("Artist")),
                           albumImage = reader.GetString("AlbumImage"),
                           madePrivate = reader.GetBoolean("MadePrivate"),
                           color = reader.GetString("Color"),
                           songs = GetSongsByAlbum(albumID)
                           };
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
                         allAlbums.Add(new Albums {
                             albumId = reader.GetInt32("AlbumID"),
                             albumTitle = reader.GetString("AlbumTitle"),
                             albumType = (AlbumType)reader.GetInt32("AlbumType"),
                             artist = ShowArtistByID(reader.GetInt32("Artist")),
                             albumImage = reader.GetString("AlbumImage"),
                             madePrivate = reader.GetBoolean("MadePrivate"),
                             color = reader.GetString("Color"),
                             songs = GetSongsByAlbum(reader.GetInt32("AlbumID"))
                         });
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
                    SqlCommand cmd = new SqlCommand("GetAllAlbumsByArtistSP", conn);
                    cmd.Parameters.AddWithValue("@Artist", artistID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allAlbums.Add(new Albums {
                            albumId = reader.GetInt32("AlbumID"),
                            albumTitle = reader.GetString("AlbumTitle"),
                            albumType = (AlbumType)reader.GetInt32("AlbumType"),
                            artist = ShowArtistByID(reader.GetInt32("Artist")),
                            albumImage = reader.GetString("AlbumImage"),
                            madePrivate = reader.GetBoolean("MadePrivate"),
                            color = reader.GetString("Color"),
                            songs = GetSongsByAlbum(reader.GetInt32("AlbumID")),
                        });
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
        public void CreateSong(Songs song)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreateSongSP", conn);
                    cmd.Parameters.AddWithValue("@SongTitle", song.title);
                    cmd.Parameters.AddWithValue("@ArtistID", song.artistId);
                    cmd.Parameters.AddWithValue("@AlbumID", song.albumId);
                    cmd.Parameters.AddWithValue("@SongLength", song.length.TotalSeconds);
                    cmd.Parameters.AddWithValue("@SoundFile", song.filepath);
                    cmd.Parameters.AddWithValue("@MadePrivate", song.isPrivate);
                    cmd.Parameters.AddWithValue("@GenreID", song.genre.GenreId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void UpdateSong(Songs updatedSong)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateSongSP", conn);
                    cmd.Parameters.AddWithValue("@SongID", updatedSong.songId);
                    cmd.Parameters.AddWithValue("@SongTitle", updatedSong.title);
                    cmd.Parameters.AddWithValue("@ArtistID", updatedSong.artistId);
                    cmd.Parameters.AddWithValue("@AlbumID", updatedSong.albumId);
                    cmd.Parameters.AddWithValue("@SongLength", updatedSong.length);
                    cmd.Parameters.AddWithValue("@SoundFile", updatedSong.filepath);
                    cmd.Parameters.AddWithValue("@MadePrivate", updatedSong.isPrivate);
                    cmd.Parameters.AddWithValue("@GenreID", updatedSong.genre);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Songs> GetSongsByAlbum(int albumID)
        {
            try
            {
                List<Songs> albumSongs = new List<Songs>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSongsByAlbumSP", conn);
                    cmd.Parameters.AddWithValue("@AlbumID", albumID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        albumSongs.Add(new Songs {
                            songId = reader.GetInt32("SongID"),
                            title = reader.GetString("SongTitle"),
                            length = TimeSpan.FromSeconds(reader.GetInt32("SongLength")),
                            albumId = reader.GetInt32("AlbumID"),
                            artistId = reader.GetInt32("ArtistID"),
                            timesPlayed = reader.GetInt32("TimesListened"),
                            filepath = reader.GetString("SoundFile"),
                            albumTrackNr = reader.GetInt32("AlbumTrackNumber"),
                            isPrivate = reader.GetBoolean("MadePrivate"),
                            genre = GetGenreByID(reader.GetInt32("GenreID")),
                        });
                    }
                }
                return albumSongs;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public List<Songs> GetSongsByArtist(int artistID)
        {
            try
            {
                List<Songs> artistSongs = new List<Songs>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSongsByArtistSP", conn);
                    cmd.Parameters.AddWithValue("@ArtistID", artistID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        artistSongs.Add(new Songs {
                            songId = reader.GetInt32("SongID"),
                            title = reader.GetString("SongTitle"),
                            length = TimeSpan.FromSeconds(reader.GetInt32("SongLength")),
                            albumId = reader.GetInt32("AlbumID"),
                            artistId = reader.GetInt32("ArtistID"),
                            timesPlayed = reader.GetInt32("TimesListened"),
                            filepath = reader.GetString("SoundFile"),
                            albumTrackNr = reader.GetInt32("AlbumTrackNumber"),
                            isPrivate = reader.GetBoolean("MadePrivate"),
                            genre = GetGenreByID(reader.GetInt32("GenreID"))
                        });
                    }
                }
                return artistSongs;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public Songs GetSongByID(int songID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSongByIDSP", conn);
                    cmd.Parameters.AddWithValue("@SongID", songID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Songs {
                            songId = reader.GetInt32("SongID"),
                            title = reader.GetString("SongTitle"),
                            length = TimeSpan.FromSeconds(reader.GetInt32("SongLength")),
                            albumId = reader.GetInt32("AlbumID"),
                            artistId = reader.GetInt32("ArtistID"),
                            timesPlayed = reader.GetInt32("TimesListened"),
                            filepath = reader.GetString("SoundFile"),
                            albumTrackNr = reader.GetInt32("AlbumTrackNumber"),
                            isPrivate = reader.GetBoolean("MadePrivate"),
                            genre = GetGenreByID(reader.GetInt32("GenreID"))
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public List<Songs> GetSongsByGenre(int genreID)
        {
            try
            {
                List<Songs> albumSongs = new List<Songs>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetSongsByGenreSP", conn);
                    cmd.Parameters.AddWithValue("@GenreID", genreID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        albumSongs.Add(new Songs
                        {
                            songId = reader.GetInt32("SongID"),
                            title = reader.GetString("SongTitle"),
                            length = TimeSpan.FromSeconds(reader.GetInt32("SongLength")),
                            albumId = reader.GetInt32("AlbumID"),
                            artistId = reader.GetInt32("ArtistID"),
                            timesPlayed = reader.GetInt32("TimesListened"),
                            filepath = reader.GetString("SoundFile"),
                            albumTrackNr = reader.GetInt32("AlbumTrackNumber"),
                            isPrivate = reader.GetBoolean("MadePrivate"),
                            genre = GetGenreByID(reader.GetInt32("GenreID")),
                        });
                    }
                }
                return albumSongs;
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        /*------------------------------------------Playlist--------------------------------------------------*/
        public int CreatePlaylist(int userID, string playlistName, string playlistImage, string playlistColor, bool madePrivate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreatePlaylistSP", conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@PlaylistName", playlistName);
                    cmd.Parameters.AddWithValue("@PlaylistImage", playlistImage);
                    cmd.Parameters.AddWithValue("@PlaylistColor", playlistColor);
                    cmd.Parameters.AddWithValue("@MadePrivate", madePrivate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int id = (int)cmd.ExecuteScalar();
                    return id;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public void EditPlaylist(Playlists editedPlaylist)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("EditPlaylistSP", conn);
                    cmd.Parameters.AddWithValue("@PlaylistID", editedPlaylist.PlaylistId);
                    cmd.Parameters.AddWithValue("@PlaylistName", editedPlaylist.PlaylistName);
                    cmd.Parameters.AddWithValue("@PlaylistImage", editedPlaylist.PlaylistImage);
                    cmd.Parameters.AddWithValue("@PlaylistColor", editedPlaylist.PlaylistsColor);
                    cmd.Parameters.AddWithValue("@MadePrivate", editedPlaylist.MadePrivate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public void DeletePlaylist(int playlistId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DeletePlaylistByIDSP", conn);
                cmd.Parameters.AddWithValue("@PlaylistID", playlistId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }
        public List<Playlists> GetAllUserPlaylists(int userId)
        {
            List<Playlists> allUserPlaylists = new List<Playlists>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetAllUserPlaylists", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    allUserPlaylists.Add(new Playlists
                    {
                        PlaylistId = reader.GetInt32("PlaylistID"),
                        PlaylistName = reader.GetString("PlaylistName"),
                        PlaylistImage = reader.GetString("PlaylistImage"),
                        MadePrivate = reader.GetBoolean("MadePrivate"),
                        UserId = userId,
                        Songs = GetPlaylistSongs(reader.GetInt32("PlaylistID"))

                    });
                }
            }
            return allUserPlaylists;
        }
        public void AddSongToPlaylist(int songId, int playlistId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddSongToPlaylistSP", conn);
                cmd.Parameters.AddWithValue("@SongID", songId);
                cmd.Parameters.AddWithValue("@PlaylistID", playlistId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }
        public void RemoveSongFromPlaylist(int playlistSongId, int playlistId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("RemoveSongFromPlaylistSP", conn);
                cmd.Parameters.AddWithValue("@PlaylistID", playlistId);
                cmd.Parameters.AddWithValue("@PlaylistSongID", playlistSongId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }
        public List<Songs> GetPlaylistSongs(int playlistID)
        {
            List<Songs> playlistSongs = new List<Songs>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetPlaylistSongsSP", conn);
                cmd.Parameters.AddWithValue("@PlaylistID", playlistID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    playlistSongs.Add(new Songs
                    {
                        songId = reader.GetInt32("SongID"),
                        title = reader.GetString("SongTitle"),
                        length = TimeSpan.FromSeconds(reader.GetInt32("SongLength")),
                        albumId = reader.GetInt32("AlbumID"),
                        artistId = reader.GetInt32("ArtistID"),
                        timesPlayed = reader.GetInt32("TimesListened"),
                        filepath = reader.GetString("SoundFile"),
                        albumTrackNr = reader.GetInt32("AlbumTrackNumber"),
                        isPrivate = reader.GetBoolean("MadePrivate"),
                        genre = GetGenreByID(reader.GetInt32("GenreID")),
                        playlistSongId = reader.GetInt32("PlaylistSongID")
                    });
                }
            }
            return playlistSongs;
        }
        public Playlists GetPlaylistByID(int playlistID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetPlaylistByIDSP", conn);
                cmd.Parameters.AddWithValue("@PlaylistID", playlistID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    return new Playlists
                    {
                        PlaylistId = playlistID,
                        UserId = reader.GetInt32("UserID"),
                        PlaylistName = reader.GetString("PlaylistName"),
                        PlaylistImage = reader.GetString("PlaylistImage"),
                        PlaylistsColor = reader.GetString("PlaylistColor"),
                        MadePrivate = reader.GetBoolean("MadePrivate"),
                        Songs = GetPlaylistSongs(playlistID)
                    };
                }
            }
            return null;
        }
        /*------------------------------------------Genre--------------------------------------------------*/
        public void CreateGenre(string genreName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CreateGenreSP", conn);
                    cmd.Parameters.AddWithValue("@GenreName", genreName);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public List<Genres> GetAllGenres()
        {
            List<Genres> allGenres = new List<Genres>();

            using (SqlConnection conn = new SqlConnection( connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAllGenresSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allGenres.Add(new Genres
                        {
                            GenreId = reader.GetInt32("GenreID"),
                            GenreName = reader.GetString("GenreName"),
                            Deleted = reader.GetBoolean("Deleted")
                        });
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                        return allGenres;
            }
        }
        public List<Genres> GetAllUndeletedGenres()
        {
            List<Genres> allGenres = new List<Genres>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAllUndeletedGenresSP", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allGenres.Add(new Genres
                        {
                            GenreId = reader.GetInt32("GenreID"),
                            GenreName = reader.GetString("GenreName"),
                            Deleted = reader.GetBoolean("Deleted")
                        });
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return allGenres;
            }
        }
        public Genres GetGenreByID(int genreID)
        {
            using (SqlConnection conn = new SqlConnection( connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetGenreByIDSP", conn);
                    cmd.Parameters.AddWithValue("@GenreID", genreID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Genres {
                            GenreId = genreID,
                            GenreName = reader.GetString("GenreName"),
                            Deleted = reader.GetBoolean("Deleted")
                        };
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;
        }
        public void DeleteGenreByID(int genreID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DeleteGenreByIDSP", conn);
                    cmd.Parameters.AddWithValue("@GenreID", genreID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        /*------------------------------------------Subscriptions--------------------------------------------------*/
    }
}
