using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace System2Interaction
{
    public static class RequestFactory
    {
        public static async Task<string> MakeRequest(Uri requestAddress)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestAddress);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.ContentLength = 0;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string responseText;
            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    responseText = await new StreamReader(stream).ReadToEndAsync();
                }
            }
            return responseText;
        }
    }
}