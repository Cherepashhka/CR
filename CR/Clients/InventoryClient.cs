using CR.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
namespace CR.Clients
{
    public class InventoryClient
    {
        private static string _apikey;
        private static string _apihost;

        public InventoryClient()
        {
            _apikey = Const.InventoryApiKey;
            _apihost = Const.InventoryApiHost;
        }
        public async Task<List<InventoryItems>> GetInventoryInfo (string steamID)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://steamwebapi1.p.rapidapi.com/steam/api/inventory?steam_id={steamID}&parse=1&group=1&game=csgo&language=english&currency=usd"),
                Headers =
            {
                { "x-rapidapi-key", _apikey },
                { "x-rapidapi-host", _apihost },
            },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                var answer = JsonConvert.DeserializeObject<List<InventoryItems>>(body);

                return answer;
            }
        }
    }
}
