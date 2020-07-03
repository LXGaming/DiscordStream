using ConcurrentCollections;
using LXGaming.DiscordStream.Configuration.Category;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration {

    public class Config {

        [JsonProperty(PropertyName = "account")]
        public AccountCategory AccountCategory = new AccountCategory();

        [JsonProperty(PropertyName = "general")]
        public GeneralCategory GeneralCategory = new GeneralCategory();

        [JsonProperty(PropertyName = "guilds")]
        public ConcurrentHashSet<GuildCategory> GuildCategories = new ConcurrentHashSet<GuildCategory>();

        [JsonProperty(PropertyName = "integration")]
        public IntegrationCategory IntegrationCategory = new IntegrationCategory();

        [JsonProperty(PropertyName = "message")]
        public MessageCategory MessageCategory = new MessageCategory();

        [JsonProperty(PropertyName = "service")]
        public ServiceCategory ServiceCategory = new ServiceCategory();

        [JsonProperty(PropertyName = "web")]
        public WebCategory WebCategory = new WebCategory();
    }
}