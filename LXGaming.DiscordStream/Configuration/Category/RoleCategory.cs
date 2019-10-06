using System.Collections.Generic;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class RoleCategory {

        [JsonProperty(PropertyName = "id")]
        public ulong Id;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Unknown";

        [JsonProperty(PropertyName = "permissions")]
        public HashSet<string> Permissions = new HashSet<string>();
    }
}