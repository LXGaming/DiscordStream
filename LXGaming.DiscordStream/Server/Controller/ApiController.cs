using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LXGaming.DiscordStream.Server.Controller {

    [Route("api")]
    public class ApiController : ControllerBase {

        [Route("webhooks")]
        public JsonResult Index() {

            dynamic json = new JObject();
            json["id"] = DiscordStream.Id;
            json["name"] = DiscordStream.Name;
            json["version"] = DiscordStream.Version;
            json["authors"] = DiscordStream.Authors;
            json["source"] = DiscordStream.Source;
            json["website"] = DiscordStream.Website;

            return new JsonResult(json);
        }
    }
}