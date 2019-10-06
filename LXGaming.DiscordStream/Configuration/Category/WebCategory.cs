using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class WebCategory {

        public const int DefaultConnectTimeout = 15000;
        public const int DefaultReadTimeout = 15000;
        public const int DefaultWriteTimeout = 15000;

        [JsonProperty(PropertyName = "followCallback")]
        public string FollowCallback = "http://localhost:8080/api/webhooks/incoming/twitch/followers";

        [JsonProperty(PropertyName = "streamCallback")]
        public string StreamCallback = "http://localhost:8080/api/webhooks/incoming/twitch/stream";

        [JsonProperty(PropertyName = "url")]
        public string Url = "http://localhost:8080/";

        [JsonProperty(PropertyName = "connectTimeout")]
        public int ConnectTimeout = DefaultConnectTimeout;

        [JsonProperty(PropertyName = "readTimeout")]
        public int ReadTimeout = DefaultReadTimeout;

        [JsonProperty(PropertyName = "writeTimeout")]
        public int WriteTimeout = DefaultWriteTimeout;
    }
}