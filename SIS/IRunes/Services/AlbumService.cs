using System.Collections.Generic;
using System.Linq;
using IRunes.Data;
using IRunes.Models;
using IRunes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {
        public ICollection<Album> GetAllAlbums()
        {
            using (var db = new IRunesDbContext())
            {
                return db.Albums.Include(a => a.Tracks).ToArray();
            }
        }

        public void CreateAlbum(string albumName, string albumCover)
        {
            var album = new Album { Name = albumName, Cover = albumCover };
            using (var db = new IRunesDbContext())
            {
                db.Albums.Add(album);
                db.SaveChanges();
            }
        }

        public Album GetAlbumById(string albumId)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Albums.FirstOrDefault(a => a.Id.ToString() == albumId);
            }
        }

        public ICollection<Track> GetAlbumTracks(string albumId)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Tracks.Where(t => t.AlbumId.ToString() == albumId).ToArray();
            }
        }

        public bool AlbumExists(string albumId)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Albums.Any(a => a.Id.ToString() == albumId);
            }
        }

        public decimal GetPrice(string albumId)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Albums.FirstOrDefault(a => a.Id.ToString() == albumId).Tracks.Sum(t => t.Price) * 0.87M;
            }
        }
    }
}