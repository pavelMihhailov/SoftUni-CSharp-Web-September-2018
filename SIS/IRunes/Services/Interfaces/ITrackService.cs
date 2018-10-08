using IRunes.Models;

namespace IRunes.Services.Interfaces
{
    public interface ITrackService
    {
        void CreateTrack(string name, string albumId, string link, decimal price);

        Track GetTrackById(string trackId);
    }
}