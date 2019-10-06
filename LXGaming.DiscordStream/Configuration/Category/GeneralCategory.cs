using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class GeneralCategory {

        [JsonProperty(PropertyName = "debug")]
        public bool Debug;

        [JsonProperty(PropertyName = "commandPrefix")]
        public string CommandPrefix = "!Stream";
    }
}