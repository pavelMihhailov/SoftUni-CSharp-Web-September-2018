using System.Collections.Generic;
using IRunes.Controllers;
using IRunes.Extensions;
using IRunes.Models;
using IRunes.Services;
using IRunes.Services.Interfaces;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.App.Controllers
{
    public class TrackController : BaseController
    {
        private readonly IAlbumService albumService;
        private readonly ITrackService trackService;

        public TrackController()
        {
            this.albumService = new AlbumService();
            this.trackService = new TrackService();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return this.Error("No album id specified", HttpResponseStatusCode.BadRequest, request);
            }

            var albumId = request.QueryData["albumId"].ToString();

            if (!this.albumService.AlbumExists(albumId))
            {
                return this.Error("Album not found", HttpResponseStatusCode.NotFound, request);
            }
            

            var viewBag = new Dictionary<string, string> { { "AlbumId", albumId} };

            return this.View("Tracks/Create", request, viewBag);
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return this.Error("No album id specified", HttpResponseStatusCode.BadRequest, request);
            }

            var name = request.FormData["name"].ToString();
            var link = request.FormData["link"].ToString();
            var priceString = request.FormData["price"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link)
                                                || string.IsNullOrWhiteSpace(priceString))
            {
                return this.Error("Name, link and price cannot be empty!", HttpResponseStatusCode.BadRequest, request);
            }

            if (!decimal.TryParse(priceString, out decimal price))
            {
                return this.Error("Invalid price", HttpResponseStatusCode.BadRequest, request);
            }

            var albumId = request.QueryData["albumId"].ToString();

            var album = this.albumService.GetAlbumById(albumId);

            if (album == null)
            {
                return this.Error("Album not found", HttpResponseStatusCode.NotFound, request);
            }

            this.trackService.CreateTrack(name, albumId, link, price);

            return this.Redirect("/Albums/Details?id=" + albumId);
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                return this.Error("No track id specified", HttpResponseStatusCode.BadRequest, request);
            }

            var trackId = (string)request.QueryData["id"];

            Track track = this.trackService.GetTrackById(trackId);

            if (track == null)
            {
                return this.Error("Track not found", HttpResponseStatusCode.NotFound, request);
            }

            var viewBag = new Dictionary<string, string>
                              {
                                  {"Name", track.Name},
                                  {"Link", track.Link.Replace("watch?v=", "embed/")},
                                  {"AlbumId", track.AlbumId.ToString()},
                                  {"Price", track.Price.ToString("F2")}
                              };

            return this.View("Tracks/Details", request, viewBag);
        }
    }
}