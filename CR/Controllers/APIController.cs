using CR.Clients;
using CR.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace CR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkinInfoController : ControllerBase
    {
        private readonly ILogger<SkinInfoController> _logger;

        public SkinInfoController (ILogger<SkinInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/GetSkinPrice")]
        public SkinInfo ThrowSkinInfo (string skinName)
        {
            SkinClient client = new SkinClient();
            SkinInfo? skinInfo = client.GetSkinInfo(skinName).Result;
            return skinInfo;
        }
        [HttpPost("/NewPreference")]
        public async void InsertSkinInfo(string serskinname)
        {
            string skinName = JsonConvert.DeserializeObject<string>(serskinname);
            SkinInfo skinInfo = ThrowSkinInfo(skinName);    
            DatabaseClient databaseconnection = new DatabaseClient();
            await databaseconnection.InsertNewPreference(skinInfo);
            return;
        }
        [HttpGet("/Inventory")]
        public string InventoryPrice(string steamID)
        {
            InventoryClient client = new InventoryClient();
            List<InventoryItems>? inventoryItems = client.GetInventoryInfo(steamID).Result;
            double totalprice = 0;
            foreach (InventoryItems item in inventoryItems)
                totalprice += item.priceavg * item.count;

            totalprice = Math.Round(totalprice, 3);
            string result = $"Загальна ціна інвентаря: {totalprice} доларів\nНайдорожчий предмет: {inventoryItems[0].marketname}, його ціна - {inventoryItems[0].priceavg} доларів.\nПоверніться на початок з /start";
            return result;
        }
        [HttpDelete("/DeletePreference")]
        public async void DeleteItemFromPrefList(string name)
        {
            name = name.Replace('_', ' ');
            name = name.Insert(name.IndexOf(' '), " |");
            Console.WriteLine(name);
            DatabaseClient databaseconnection = new DatabaseClient();
            await databaseconnection.DeletePrefence(name);
            return;
        }
        [HttpGet("/PreferenceList")]
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
        [HttpDelete("/DeleteAllPreference")]
        public async void DeleteAllFromPrefList()
        {
            DatabaseClient databaseconnection = new DatabaseClient();
            await databaseconnection.DeleteAllPreference();
            return;
        }
    }
}
