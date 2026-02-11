using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public List<Songs> GetSongsByGenre(int genreID) => connection.GetSongsByGenre(genreID);

        public int CreatePlaylist(int userID, string playlistName, string playlistImage, string playlistColor, bool madePrivate) => connection.CreatePlaylist(userID, playlistName, playlistImage, playlistColor, madePrivate);
        public void EditPlaylist(Playlists editedPlaylist) => connection.EditPlaylist(editedPlaylist);
        public void DeletePlaylist(int playlistId) => connection.DeletePlaylist(playlistId);
        public Playlists GetPlaylistByID(int playlistID) => connection.GetPlaylistByID(playlistID);
        public List<Playlists> GetAllUserPlaylists(int userId) => connection.GetAllUserPlaylists(userId);
        public void AddSongToPlaylist(int songId, int playlistId) => connection.AddSongToPlaylist(songId, playlistId);
        public void RemoveSongFromPlaylist(int playlistSongId, int playlistId) => connection.RemoveSongFromPlaylist(playlistSongId, playlistId);
        public List<Songs> GetPlaylistSongs(int playlistID) => connection.GetPlaylistSongs(playlistID);


        public void CreateGenre(string genreName) => connection.CreateGenre(genreName);
        public List<Genres> GetAllGenres() => connection.GetAllGenres();
        public List<Genres> GetAllUndeletedGenres() => connection.GetAllUndeletedGenres();
        public Genres GetGenreByID(int genreID) => connection.GetGenreByID(genreID);
        public void DeleteGenreByID(int genreID) => connection.DeleteGenreByID(genreID);









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
        public TimeSpan GetTotalPlaylistTime(int playlistID)
        {
            Playlists playlist = GetPlaylistByID(playlistID);
            TimeSpan totalPlaylistTime = TimeSpan.Zero;
            foreach (Songs song in playlist.Songs)
            {
                totalPlaylistTime += song.length;
            }
            return totalPlaylistTime;
        }
        public string MakeQueueStart(int songId)
        {
            Songs pressedSong = GetSongByID(songId);
            Albums fromAlbum = GetAlbumByID(pressedSong.albumId);
            StringBuilder sb = new StringBuilder();
            sb.Append(pressedSong.songId.ToString());
            /*Console.WriteLine(fromAlbum.songs.IndexOf(pressedSong));
            foreach (Songs song in fromAlbum.songs)
            {
                Console.WriteLine(fromAlbum.songs.IndexOf(pressedSong));
                Console.WriteLine(fromAlbum.songs.IndexOf(song));
                if (fromAlbum.songs.IndexOf(song) > fromAlbum.songs.IndexOf(pressedSong))
                {

                    sb.Append(",");
                    sb.Append(song.songId);
                }
            }*/
            return sb.ToString();
            //Console.WriteLine(Song);
        }
        public string AddToQueue(string? queue, int songId)
        {
            StringBuilder sb = new();

            if (queue != null)
            {
                sb.Append(queue.ToString());
                sb.Append(",");
                sb.Append(songId.ToString());
            }
            else
            {
                sb.Append(songId.ToString());
            }
            return sb.ToString();
        }
    }
}
