using SIS.HTTP.Enums;

namespace SIS.HTTP.Extensions
{
    public class HttpResponseStatusExtensions
    {
        public HttpResponseStatusCode GetResponseLine(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    return HttpResponseStatusCode.Ok;
                case 201:
                    return HttpResponseStatusCode.Created;
                case 302:
                    return HttpResponseStatusCode.Found;
                case 303:
                    return HttpResponseStatusCode.SeeOther;
                case 400:
                    return HttpResponseStatusCode.BadRequest;
                case 401:
                    return HttpResponseStatusCode.Unauthorized;
                case 403:
                    return HttpResponseStatusCode.Forbidden;
                case 404:
                    return HttpResponseStatusCode.NotFound;
                case 500:
                    return HttpResponseStatusCode.InternalServerError;
                default:
                    return HttpResponseStatusCode.NotFound;
            }
        }
    }
}
