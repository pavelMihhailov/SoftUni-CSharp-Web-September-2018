using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (!headers.ContainsKey(header.Key))
            {
                headers.Add(header.Key, header);
            }
        }

        public bool ContainsHeader(string key)
        {
            return headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (!headers.ContainsKey(key))
            {
                return null;
            }
            return headers[key];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var header in headers.Values)
            {
                sb.AppendLine(header.ToString());
            }

            return sb.ToString().Trim();
        }
    }
}
