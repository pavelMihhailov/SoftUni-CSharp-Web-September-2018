using System.Collections.Generic;
using IRunes.Models;

namespace IRunes.Services.Interfaces
{
    public interface IAlbumService
    {
        ICollection<Album> GetAllAlbums();

        void CreateAlbum(string albumName, string albumCover);

        Album GetAlbumById(string albumId);

        ICollection<Track> GetAlbumTracks(string albumId);

        bool AlbumExists(string albumId);

        decimal GetPrice(string albumId);
    }
}