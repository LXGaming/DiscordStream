using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LXGaming.DiscordStream.Manager;
using LXGaming.DiscordStream.Util;
using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;

namespace LXGaming.DiscordStream.Integration.Twitch {

    public class TwitchIntegration {

        public static TwitchAPI TwitchApi { get; private set; }

        public static void Prepare() {
            var twitchIntegrationCategory = DiscordStream.Instance.Config.IntegrationCategory.TwitchIntegrationCategory;
            CreateTwitchApi(twitchIntegrationCategory.Id, twitchIntegrationCategory.Token);

            Task.Run(() => {
                try {
                    var duration = TimeSpan.FromHours(72);
                    while (true) {
                        Subscribe(duration);
                        if (DiscordStream.Instance.State.WaitOne(duration, false)) {
                            break;
                        }
                    }
                } catch (System.Exception ex) {
                    DiscordStream.Instance.Logger.Warn("Encountered an error during subscribe task", ex);
                }
            });
        }

        private static void Subscribe(TimeSpan duration) {
            var config = DiscordStream.Instance.Config;
            if (config == null) {
                return;
            }

            var users = TwitchApi.Helix.Users.GetUsersAsync(null, new List<string>(config.IntegrationCategory.TwitchIntegrationCategory.Channels)).Result;
            if (users == null) {
                DiscordStream.Instance.Logger.Warn("Failed to get users");
                return;
            }

            foreach (var user in users.Users) {
                Toolbox.Put(DataManager.TwitchUserCache, Convert.ToInt64(user.Id), user);

                DiscordStream.Instance.Logger.Info("Subscribing to {} ({})", user.Login, user.Id);
                TwitchApi.Helix.Webhooks.StreamUpDownAsync(
                    config.WebCategory.StreamCallback + "?user_id=" + user.Id,
                    WebhookCallMode.Subscribe,
                    user.Id,
                    duration,
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
            } catch (System.Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while creating TwitchApi", ex);
            }
        }
    }
}