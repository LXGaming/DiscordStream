using Discord;
using LXGaming.DiscordStream.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class AccountCategory {

        [JsonProperty(PropertyName = "id")]
        public ulong Id;

        [JsonProperty(PropertyName = "name")]
        public string Name = "Unknown";

        [JsonProperty(PropertyName = "token")]
        public string Token = "";

        [JsonProperty(PropertyName = "activityName")]
        public string ActivityName = Reference.Name;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "activityType")]
        public ActivityType ActivityType = ActivityType.Playing;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "userStatus")]
        public UserStatus UserStatus = UserStatus.Online;
    }
}