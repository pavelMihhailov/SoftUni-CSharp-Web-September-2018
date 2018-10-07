using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            Validator.ThrowIfNull(statusCode, nameof(statusCode));

            this.Headers = new HttpHeaderCollection();
            Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            Validator.ThrowIfNull(header, nameof(header));
            Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            Validator.ThrowIfNull(cookie, nameof(cookie));
            Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString());
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode}")
                .AppendLine(Headers.ToString());

            if (this.Cookies.HasCookies())
            {
                foreach (var httpCookie in Cookies)
                {
                    result.AppendLine($"Set-Cookie: {httpCookie}");
                }
            }

            return result.ToString().Trim();
        }
    }
}
