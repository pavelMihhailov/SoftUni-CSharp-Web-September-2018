using System.Collections.Generic;
using IRunes.Extensions;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.View("Home/IndexLoggedOut", request);
            }

            var dict = new Dictionary<string, string>
                           {
                               {"Username", request.GetUsername()}
                           };
            return this.View("Home/IndexLoggedIn", request, dict);

        }
    }
}