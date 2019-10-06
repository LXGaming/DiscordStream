using LXGaming.DiscordStream.Configuration.Category.Integration;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class IntegrationCategory {

        [JsonProperty(PropertyName = "mixer")]
        public MixerIntegrationCategory MixerIntegrationCategory = new MixerIntegrationCategory();

        [JsonProperty(PropertyName = "twitch")]
        public TwitchIntegrationCategory TwitchIntegrationCategory = new TwitchIntegrationCategory();
    }
}