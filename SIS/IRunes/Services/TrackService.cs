using System;
using System.Linq;
using IRunes.Data;
using IRunes.Models;
using IRunes.Services.Interfaces;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        public void CreateTrack(string name, string albumId, string link, decimal price)
        {
            var track = new Track{AlbumId = Guid.Parse(albumId), Name = name, Link = link, Price = price};
            using (var db = new IRunesDbContext())
            {
                db.Tracks.Add(track);
                db.SaveChanges();
            }
        }

        public Track GetTrackById(string trackId)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Tracks.FirstOrDefault(t => t.Id.ToString() == trackId);
            }
        }
    }
}