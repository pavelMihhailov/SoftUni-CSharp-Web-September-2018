﻿using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            Headers.Add(new HttpHeader("Content-Type", "text/plain"));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
