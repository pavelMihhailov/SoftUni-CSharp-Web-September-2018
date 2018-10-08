using IRunes.App.Controllers;
using IRunes.Controllers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace IRunes.App
{
    public class Launcher
    {
        static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            ConfigureRoutes(serverRoutingTable);

            var server = new Server(1984, serverRoutingTable);
            server.Run();
        }

        private static void ConfigureRoutes(ServerRoutingTable serverRoutingTable)
        {
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = req => new HomeController().Index(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Home/Index"] = req => new RedirectResult("/");

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Login"] = req => new UsersController().Login(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Register"] = req => new UsersController().Register(req);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Login"] = req => new UsersController().DoLogin(req);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Register"] = req => new UsersController().DoRegister(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Logout"] = req => new UsersController().Logout(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/All"] = req => new AlbumController().All(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Create"] = req => new AlbumController().Create(req);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Albums/Create"] = req => new AlbumController().DoCreate(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Details"] = req => new AlbumController().Details(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Create"] = req => new TrackController().Create(req);

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Tracks/Create"] = req => new TrackController().DoCreate(req);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Details"] = req => new TrackController().Details(req);
        }
    }
}
