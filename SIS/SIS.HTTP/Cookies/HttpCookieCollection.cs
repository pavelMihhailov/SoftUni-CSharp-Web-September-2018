using SIS.HTTP.Common;
using System.Collections;
using System.Collections.Generic;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            Validator.ThrowIfNull(cookie, nameof(cookie));
            cookies.Add(cookie.Key, cookie);
        }

        public bool ContainsCookie(string key)
        {
            Validator.ThrowIfNull(key, nameof(key));
            return cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            Validator.ThrowIfNull(key, nameof(key));
            return cookies.GetValueOrDefault(key, null);
        }
        
        public bool HasCookies()
        {
            return cookies.Count > 0;
        }

        public override string ToString()
        {
            return string.Join("; ", cookies.Values);
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
