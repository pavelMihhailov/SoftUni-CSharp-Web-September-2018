using SIS.HTTP.Requests;

namespace IRunes.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsLoggedIn(this IHttpRequest request)
            => request.Session.ContainsParameter("username");

        public static string GetUsername(this IHttpRequest request)
        {
            if (!request.Session.ContainsParameter("username"))
            {
                return null;
            }

            return (string)request.Session.GetParameter("username");
        }
    }
}