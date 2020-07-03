using System.Collections.Concurrent;
using LXGaming.DiscordStream.Entity;
using Newtonsoft.Json;

namespace LXGaming.DiscordStream.Configuration.Category {

    public class MessageCategory {

        [JsonProperty(PropertyName = "colors")]
        public ConcurrentDictionary<Color, string> Colors = new ConcurrentDictionary<Color, string>();

        [JsonProperty(PropertyName = "deleteInterval")]
        public int DeleteInterval = 60000;

        [JsonProperty(PropertyName = "deleteInvoking")]
        public bool DeleteInvoking = true;

        [JsonProperty(PropertyName = "deleteMessages")]
        public bool DeleteMessages = true;

        [JsonProperty(PropertyName = "sendTyping")]
        public bool SendTyping = true;
    }
}