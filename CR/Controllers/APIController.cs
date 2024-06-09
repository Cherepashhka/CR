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
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _logger;

        public APIController (ILogger<APIController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/APIController/GetSkinPrice")]
        public SkinInfo ThrowSkinInfo (string skinName)
        {
            SkinClient client = new SkinClient();
            SkinInfo? skinInfo = client.GetSkinInfo(skinName).Result;
            return skinInfo;
        }
        [HttpGet("/APIController/Inventory")]
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
    }
}
