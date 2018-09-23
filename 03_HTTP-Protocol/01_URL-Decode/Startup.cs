using System;
using System.Net;

namespace _03_HTTP_Protocol
{
    public class Startup
    {
        public static void Main()
        {
            string encodedUrl = Console.ReadLine();
            string decodedUrl = WebUtility.UrlDecode(encodedUrl);

            Console.WriteLine(decodedUrl);
        }
    }
}
