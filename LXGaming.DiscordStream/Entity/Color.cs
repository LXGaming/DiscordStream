using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Entity {

    public enum Color {

        [JsonProperty(PropertyName = "default")] Default,
        [JsonProperty(PropertyName = "error")] Error,
        [JsonProperty(PropertyName = "success")] Success,
        [JsonProperty(PropertyName = "warning")] Warning
    }
}