using LXGaming.DiscordStream.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LXGaming.DiscordStream.Server.Controller {

    [Route("api")]
    public class ApiController : ControllerBase {

        [Route("webhooks")]
        public JsonResult Index() {

            dynamic json = new JObject();
            json["id"] = Reference.Id;
            json["name"] = Reference.Name;
            json["version"] = Reference.Version;
            json["authors"] = Reference.Authors;
            json["source"] = Reference.Source;
            json["website"] = Reference.Website;

            return new JsonResult(json);
        }
    }
}