using SIS.HTTP.Common;
using System;

namespace SIS.HTTP.Cookies
{
    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays)
        {
            Validator.ThrowIfNullOrEmpty(key, nameof(key));
            Validator.ThrowIfNullOrEmpty(value, nameof(value));

            Key = key;
            Value = value;
            IsNew = true;
            Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays) : this(key, value, expires)
        {
            IsNew = isNew;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime Expires { get; set; }

        public bool IsNew { get; set; }

        public override string ToString()
        {
            return $"{Key}={Value}; Expires={Expires.ToLongTimeString()}";
        }
    }
}
