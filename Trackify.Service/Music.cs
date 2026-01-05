using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;


namespace Trackify.Service
{
    public class Music : IMusic
    {
        SQL connection;
        Other conn;
        public Music(IConfiguration configuration) => connection = new SQL(configuration);
        public int CreateAlbum(int artistID, string albumTitle, AlbumType albumType, string color) => connection.CreateAlbum(artistID, albumTitle, albumType, color);
        public void UpdateAlbum(Albums updatedAlbum) => connection.UpdateAlbum(updatedAlbum);
        public Albums GetAlbumByID(int albumID) => connection.GetAlbumByID(albumID);
        public List<Albums> ShowAllAlbums() => connection.ShowAllAlbums();
        public List<Albums> ShowAllAlbumsByArtist(int artistID) => connection.ShowAllAlbumsByArtist(artistID);

        public void CreateSong(Songs song) => connection.CreateSong(song);
        public void UpdateSong(Songs updatedSong) => connection.UpdateSong(updatedSong);
        public List<Songs> GetSongsByAlbum(int albumID) => connection.GetSongsByAlbum(albumID);
        public List<Songs> getSongsByArtist(int artistID) => connection.GetSongsByArtist(artistID);
        public Songs GetSongByID(int songID) => connection.GetSongByID(songID);

        public int CreateGenre(string genreName) => connection.CreateGenre(genreName);
        public List<Genres> GetAllGenres() => connection.GetAllGenres();
        public Genres GetGenreByID(int genreID) => connection.GetGenreByID(genreID);









        public TimeSpan GetTotalDuration(int albumID)
        {
            Albums album = GetAlbumByID(albumID);
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (Songs song in album.songs)
            {
                totalDuration += song.length;
            }
            return totalDuration;
        }
    }
}
