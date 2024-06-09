using CR.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CR.Clients
{
    public class SkinClient
    {
        private static string _apikey;
        private static string _apihost;

        public SkinClient()
        {
            _apikey = Const.SkinApiKey;
            _apihost = Const.SkinApiHost;
        }

        public async Task<SkinInfo> GetSkinInfo (string name)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://cs-skin-api.p.rapidapi.com/{name}"),
                Headers =
    {
        { "X-RapidAPI-Key", _apikey },
        { "X-RapidAPI-Host", _apihost },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                var answer = JsonConvert.DeserializeObject<SkinInfo>(body);

                return answer;
            }
        }
    }
}
