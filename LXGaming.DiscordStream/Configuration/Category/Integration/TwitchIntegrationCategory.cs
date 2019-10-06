using System.Collections.Generic;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category.Integration {

    public class TwitchIntegrationCategory {

        [JsonProperty(PropertyName = "id")]
        public string Id = "";

        [JsonProperty(PropertyName = "token")]
        public string Token = "";

        [JsonProperty(PropertyName = "notifyChannel")]
        public ulong NotifyChannel;

        [JsonProperty(PropertyName = "channels")]
        public HashSet<string> Channels = new HashSet<string>();
    }
}