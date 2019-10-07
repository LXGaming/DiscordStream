using System;
using LXGaming.DiscordStream.Manager;
using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;

namespace LXGaming.DiscordStream.Integration.Twitch {

    public class TwitchIntegration {

        public static TwitchAPI TwitchApi { get; private set; }

        public static void Prepare() {
            var config = DiscordStream.Instance.Config;
            var twitchIntegrationCategory = config.IntegrationCategory.TwitchIntegrationCategory;
            CreateTwitchApi(twitchIntegrationCategory.Id, twitchIntegrationCategory.Token);

            foreach (var channel in twitchIntegrationCategory.Channels) {
                DiscordStream.Instance.Logger.Info("Subscribing to {}", channel);
                TwitchApi.Helix.Webhooks.StreamUpDownAsync(
                    config.WebCategory.StreamCallback + "?user_id=" + channel,
                    WebhookCallMode.Subscribe,
                    channel,
                    TimeSpan.FromHours(1),
                    WebManager.Secret.ToString());
            }
        }

        private static void CreateTwitchApi(string id, string token) {
            try {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(token)) {
                    throw new ArgumentException("Id/Token cannot be blank");
                }

                TwitchApi = new TwitchAPI();
                TwitchApi.Settings.ClientId = id;
                TwitchApi.Settings.Secret = token;
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while creating TwitchApi", ex);
            }
        }
    }
}