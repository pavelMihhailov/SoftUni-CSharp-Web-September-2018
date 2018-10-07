using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Sessions;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            Validator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            FormData = new Dictionary<string, object>();
            QueryData = new Dictionary<string, object>();
            Headers = new HttpHeaderCollection();
            Cookies = new HttpCookieCollection();

            ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {

            string[] splitRequestContent = requestString
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);


            string[] requestLine = splitRequestContent[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.ParseRequestMethod(requestLine);

            this.ParseRequestUrl(requestLine);

            this.ParseRequestPath(this.Url);

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());

            this.ParseCookies();

            bool requestHasBody = splitRequestContent.Length > 1;

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);
        }

        private void ParseRequestParameters(string bodyParams, bool requesthasbody)
        {
            this.ParseQueryParameters();

            if (requesthasbody)
            {
                this.ParseFormDataParams(bodyParams);
            }


        }
        //TODO check this one!and FormDataParams!
        private void ParseQueryParameters()
        {

            if (!this.Url.Contains("?"))
            {
                return;
            }
            var queryParams = this.Url.Split(new[] { '?', '#' })
                .Skip(1)
                .Take(1)
                .ToArray()[0];

            var queryKeyValuePairs = queryParams.Split('&', StringSplitOptions.RemoveEmptyEntries);

            FillData(queryKeyValuePairs, this.QueryData);

        }

        private void ParseFormDataParams(string bodyParams)
        {
            var dataParams = bodyParams.Split('&', StringSplitOptions.RemoveEmptyEntries);

            FillData(dataParams, this.FormData);
        }

        private void FillData(IEnumerable<string> dataParams, IDictionary<string, object> data)
        {
            foreach (var queryPair in dataParams)
            {
                var queryKvp = queryPair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (queryKvp.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }

                var dataFormKey = WebUtility.UrlDecode(queryKvp[0]);
                var dataFormValue = WebUtility.UrlDecode(queryKvp[1]);

                data[dataFormKey] = dataFormValue;
            }
        }


        private void ParseHeaders(string[] headers)
        {
            var emptyLineAfterHeadersIndex = Array.IndexOf(headers, string.Empty);

            for (int i = 0; i < emptyLineAfterHeadersIndex; i++)
            {
                var currentLine = headers[i];
                var headerParts = currentLine.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }

                var headerKey = headerParts[0];
                var headerValue = headerParts[1].Trim();

                var header = new HttpHeader(headerKey, headerValue);

                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsHeader(GlobalConstants.HostHeaderKey))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
        }

        private void ParseRequestPath(string url)
        {

            this.Path = url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

        }

        private void ParseRequestUrl(string[] requestLine)
        {
            string url = requestLine[1];
            if (string.IsNullOrEmpty(url))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.Url = url;

        }

        private void ParseRequestMethod(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            string reqMethod = requestLine[0];
            bool tryParseReqMethod = Enum.TryParse(reqMethod, true, out HttpRequestMethod method);

            if (!tryParseReqMethod)
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.RequestMethod = method;
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            bool validRequest = requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment;

            return validRequest;
        }

        private void ParseCookies()
        {
            if (Headers.ContainsHeader("Cookie"))
            {
                var cookieValue = Headers.GetHeader("Cookie").Value;

                if (string.IsNullOrEmpty(cookieValue)) return;

                string[] cookies = cookieValue.Split("; ");

                foreach (var splitCookie in cookies)
                {
                    string[] cookieParts = splitCookie.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);

                    if (cookieParts.Length != 2) continue;

                    string key = cookieParts[0];
                    string value = cookieParts[1];

                    Cookies.Add(new HttpCookie(key, value, false));
                }
            }
        }
    }
}
