using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class ServiceCategory {

        public const int DefaultCorePoolSize = 5;
        public const int DefaultMaximumPoolSize = 10;
        public const int DefaultKeepAliveTime = 5;

        [JsonProperty(PropertyName = "corePoolSize")]
        public int CorePoolSize = DefaultCorePoolSize;

        [JsonProperty(PropertyName = "maximumPoolSize")]
        public int MaximumPoolSize = DefaultMaximumPoolSize;

        [JsonProperty(PropertyName = "keepAliveTime")]
        public int KeepAliveTime = DefaultKeepAliveTime;
    }
}