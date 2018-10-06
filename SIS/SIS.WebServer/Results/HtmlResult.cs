using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode statusCode) : base(statusCode)
        {
            Headers.Add(new HttpHeader("Content-Type", "text/html"));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
