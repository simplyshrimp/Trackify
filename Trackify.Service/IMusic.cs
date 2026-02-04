using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.Domain.Models;
using Trackify.Domain.Models.Enums;

namespace Trackify.Service
{
    public interface IMusic
    {
        /*albums*/
        public int CreateAlbum(int artistID, string albumtitle, AlbumType albumType, string color);
        public void UpdateAlbum(Albums updatedAlbum);
        public Albums GetAlbumByID(int albumID);
        public List<Albums> ShowAllAlbums();
        public List<Albums> ShowAllAlbumsByArtist(int artistID);

        /*songs*/
        public void CreateSong(Songs song);
        public void UpdateSong(Songs updatedSong);
        public List<Songs> GetSongsByAlbum(int albumID);
        public List<Songs> getSongsByArtist(int artistID);
        public Songs GetSongByID(int songID);
        public List<Songs> GetSongsByGenre(int genreID);

        /*playlists*/
        public int CreatePlaylist(int userID, string playlistName, string playlistImage, string playlistColor, bool madePrivate);
        public void EditPlaylist(Playlists editedPlaylist);
        public void DeletePlaylist(int playlistId);
        public Playlists GetPlaylistByID(int playlistID);
        public List<Playlists> GetAllUserPlaylists(int userId);
        public void AddSongToPlaylist(int songId, int playlistId);
        public void RemoveSongFromPlaylist(int playlistSongId, int playlistId);
        public List<Songs> GetPlaylistSongs(int playlistID);

        /*genres*/
        public void CreateGenre(string genreName);
        public List<Genres> GetAllGenres();
        public Genres GetGenreByID(int genreID);

        /*extra*/
        public TimeSpan GetTotalDuration(int albumID);
        public TimeSpan GetTotalPlaylistTime(int playlistID);
        public string MakeQueueStart(int songId);
        public string AddToQueue(string? queue, int songId);
    }
}
