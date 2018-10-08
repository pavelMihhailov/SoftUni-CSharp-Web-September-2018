using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.App.Controllers;
using IRunes.Extensions;
using IRunes.Services;
using IRunes.Services.Interfaces;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.Controllers
{
    public class AlbumController : BaseController
    {
        private readonly IAlbumService albumService;

        public AlbumController()
        {
            this.albumService = new AlbumService();
        }

        public IHttpResponse All(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var result = new StringBuilder();

            var albums = this.albumService.GetAllAlbums();

            if (albums.Count == 0)
            {
                result.Append("<em>There are currently no albums.</em>");
            }

            else
            {
                foreach (var album in albums)
                {
                    result.AppendLine($"<a href=\"/Albums/Details?id={album.Id}\">{album.Name}</a><br>");
                }
            }

            var viewBag = new Dictionary<string, string> { { "Albums", result.ToString() } };

            return this.View("Albums/All", request, viewBag);
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View("Albums/Create", request);
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var albumName = request.FormData["name"].ToString();
            var albumCover = request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(albumCover) || string.IsNullOrWhiteSpace(albumName))
            {
                return this.Error("Album name and cover cannot be empty!", HttpResponseStatusCode.BadRequest, request);
            }

            this.albumService.CreateAlbum(albumName, albumCover);

            return this.Redirect("/Albums/All");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                return this.Error("No album id specified", HttpResponseStatusCode.BadRequest, request);
            }

            var albumId = request.QueryData["id"].ToString();

            var album = this.albumService.GetAlbumById(albumId);

            if (album == null)
            {
                return this.Error("Album not found", HttpResponseStatusCode.NotFound, request);
            }

            var result = new StringBuilder();

            var tracks = this.albumService.GetAlbumTracks(albumId).ToArray();

            if (tracks.Length == 0)
            {
                result.Append("<em>There are no tracks in this album!</em>");
            }
            else
            {
                result.Append("<ol>");
                foreach (var track in tracks)
                {
                    result.AppendLine($"<li><a href=\"/Tracks/Details?id={track.Id}\">{track.Name}</a></li>");
                }

                result.Append("</ol>");
            }

            var price = this.albumService.GetPrice(albumId);

            var viewBag = new Dictionary<string, string>
                              {
                                  {"CoverUrl", album.Cover},
                                  {"Name", album.Name},
                                  {"Price", price.ToString("F2")},
                                  {"Tracks", result.ToString()},
                                  {"AlbumId", albumId}
                              };

            return this.View("Albums/Details", request, viewBag);
        }
    }
}