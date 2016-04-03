using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirController
{
    public static class WebRequest
    {
        public static async Task<string> GetDataFromServer(string url)
        {
            var uri = new Uri(url);

            var client = new HttpClient();

            var Response = await client.GetAsync(uri);

            //var statusCode = Response.StatusCode;
            //Response.EnsureSuccessStatusCode();

            var ResponseText = await Response.Content.ReadAsStringAsync();

            return ResponseText;
        }

        public static async Task<string> PostDataToServer(string url, byte param)
        {
            var uri = new Uri(url + param.ToString());

            var client = new HttpClient();

            var Response = await client.GetAsync(uri);

            //var statusCode = Response.StatusCode;
            //Response.EnsureSuccessStatusCode();

            var ResponseText = await Response.Content.ReadAsStringAsync();

            return ResponseText;
        }
    }
}
