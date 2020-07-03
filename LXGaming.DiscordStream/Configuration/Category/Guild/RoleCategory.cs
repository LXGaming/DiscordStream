using ConcurrentCollections;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category.Guild {

    public class RoleCategory {

        [JsonProperty(PropertyName = "id")]
        public ulong Id;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Unknown";

        [JsonProperty(PropertyName = "inheritable")]
        public bool Inheritable;

        [JsonProperty(PropertyName = "permissions")]
        public ConcurrentHashSet<string> Permissions = new ConcurrentHashSet<string>();
    }
}