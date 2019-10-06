using System.Collections.Generic;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class GuildCategory {

        [JsonProperty(PropertyName = "id")]
        public ulong Id;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Unknown";

        [JsonProperty(PropertyName = "roles")]
        public HashSet<RoleCategory> RoleCategories = new HashSet<RoleCategory>();

        [JsonProperty(PropertyName = "users")]
        public HashSet<UserCategory> UserCategories = new HashSet<UserCategory>();
    }
}