using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LXGaming.DiscordStream.Server.Controller {

    [Route("error")]
    public class ErrorController : ControllerBase {

        [Route("404")]
        public new JsonResult NotFound() {
            dynamic json = new JObject();
            json["message"] = "Endpoint Not Found";
            return new JsonResult(json);
        }

        [Route("{code:int}")]
        public JsonResult Error() {
            dynamic json = new JObject();
            json["message"] = "Encountered an unexpected error";
            return new JsonResult(json);
        }
    }
}