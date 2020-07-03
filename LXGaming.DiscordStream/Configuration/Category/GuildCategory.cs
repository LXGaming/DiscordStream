using ConcurrentCollections;
using LXGaming.DiscordStream.Configuration.Category.Guild;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class GuildCategory {

        [JsonProperty(PropertyName = "id")]
        public ulong Id;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Unknown";

        [JsonProperty(PropertyName = "roles")]
        public ConcurrentHashSet<RoleCategory> RoleCategories = new ConcurrentHashSet<RoleCategory>();

        [JsonProperty(PropertyName = "users")]
        public ConcurrentHashSet<UserCategory> UserCategories = new ConcurrentHashSet<UserCategory>();
    }
}