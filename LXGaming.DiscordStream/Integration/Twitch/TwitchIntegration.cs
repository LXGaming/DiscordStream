using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            Task.Run(() => {
                var users = TwitchApi.Helix.Users.GetUsersAsync(null, new List<string>(twitchIntegrationCategory.Channels)).Result;
                if (users == null) {
                    DiscordStream.Instance.Logger.Warn("Failed to get users");
                    return;
                }

                foreach (var user in users.Users) {
                    DataManager.TwitchUserCache.Add(Convert.ToInt64(user.Id), user);

                    DiscordStream.Instance.Logger.Info("Subscribing to {} ({})", user.Login, user.Id);
                    TwitchApi.Helix.Webhooks.StreamUpDownAsync(
                        config.WebCategory.StreamCallback + "?user_id=" + user.Id,
                        WebhookCallMode.Subscribe,
                        user.Id,
                        // TODO Setup auto renewal
                        TimeSpan.FromHours(48),
                        WebManager.Secret.ToString());
                }
            });
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