using CR.Clients;
using CR.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CR.Controllers
{
    public class DatabaseController : ControllerBase
    {
        private readonly ILogger<DatabaseController> _logger;
        private APIController _apiController;
        public DatabaseController(ILogger<DatabaseController> logger)
        {
            _logger = logger;
        }
        [HttpPost("/DatabaseController/NewPreference")]
        public async void InsertSkinInfo(string serskinname)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7011");
            string skinName = JsonConvert.DeserializeObject<string>(serskinname);
            var responce = await _httpClient.GetAsync($"/APIController/GetSkinPrice?skinName={skinName}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            SkinInfo skinInfo = JsonConvert.DeserializeObject<SkinInfo>(content);
            DatabaseClient databaseconnection = new DatabaseClient();
            await databaseconnection.InsertNewPreference(skinInfo);
            return;
        }
        [HttpDelete("/DatabaseController/DeletePreference")]
        public async void DeleteItemFromPrefList(string name)
        {
            name = name.Replace('_', ' ');
            name = name.Insert(name.IndexOf(' '), " |");
            Console.WriteLine(name);
            DatabaseClient databaseconnection = new DatabaseClient();
            await databaseconnection.DeletePrefence(name);
            return;
        }
        [HttpGet("/DatabaseController/PreferenceList")]
        public string GetPreferenceList()
        {
            DatabaseClient databaseconnection = new DatabaseClient();
            var preflist = databaseconnection.GetPreferenceList().Result;
            string res = "";
            int i = 0;
            foreach (var pref in preflist)
            {
                i++;
                res += $"{i}. Назва - {pref.name}\nЦіна - {pref.price} доларів\nЗброя - {pref.weapon}\n\n";
            }
            return res;
        }
        [HttpDelete("/DatabaseController/DeleteAllPreference")]
        public async void DeleteAllFromPrefList()
        {
            DatabaseClient databaseconnection = new DatabaseClient();
            await databaseconnection.DeleteAllPreference();
            return;
        }
    }
}
