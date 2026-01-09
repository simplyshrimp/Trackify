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
        public int CreateAlbum(int artistID, string albumtitle, AlbumType albumType, string color);
        public void UpdateAlbum(Albums updatedAlbum);
        public Albums GetAlbumByID(int albumID);
        public List<Albums> ShowAllAlbums();
        public List<Albums> ShowAllAlbumsByArtist(int artistID);

        public void CreateSong(Songs song);
        public void UpdateSong(Songs updatedSong);
        public List<Songs> GetSongsByAlbum(int albumID);
        public List<Songs> getSongsByArtist(int artistID);
        public Songs GetSongByID(int songID);

        public int CreateGenre(string genreName);
        public List<Genres> GetAllGenres();
        public Genres GetGenreByID(int genreID);

        public TimeSpan GetTotalDuration(int albumID);
        public string MakeQueueStart(int songId);
        public string AddToQueue(string? queue, int songId);
    }
}
