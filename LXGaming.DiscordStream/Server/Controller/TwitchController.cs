using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using LXGaming.DiscordStream.Manager;
using LXGaming.DiscordStream.Util;
using Microsoft.AspNetCore.Mvc;
using TwitchLib.Webhook;
using TwitchLib.Webhook.Models;
using Color = LXGaming.DiscordStream.Data.Color;

namespace LXGaming.DiscordStream.Server.Controller {

    public class TwitchController : ControllerBase {

        public const string TwitchUrl = "https://www.twitch.tv/";
        public static readonly Dictionary<long, string> CurrentGames = new Dictionary<long, string>();
        public static readonly Dictionary<long, string> CurrentTitles = new Dictionary<long, string>();

        [TwitchWebHook(Id = "followers")]
        public IActionResult Follow([FromBody] Follower follower) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            DiscordStream.Instance.Logger.Info("New Follower: {}", follower.Data.FromId);
            return Ok();
        }

        [TwitchWebHook(Id = "stream")]
        public IActionResult Stream([FromQuery(Name = "user_id")] string userId, [FromBody] StreamData streamData) {
            // We return OK regardless because if Twitch does send invalid data returning anything else will just cause
            // Twitch to resend the same invalid data.
            if (!ModelState.IsValid) {
                return Ok();
            }

            // Run this async so we don't keep Twitch waiting.
            Task.Run(() => ProcessStream(userId, streamData));
            return Ok();
        }

        private void ProcessStream(string userId, StreamData streamData) {
            try {
                var config = DiscordStream.Instance.Config;
                if (config == null) {
                    DiscordStream.Instance.Logger.Warn("Config is unavailable");
                    return;
                }

                var channel = Toolbox.GetTextChannel(AccountManager.DiscordClient, config.IntegrationCategory.TwitchIntegrationCategory.NotifyChannel);
                if (channel == null) {
                    DiscordStream.Instance.Logger.Warn("Failed to find Notify channel");
                    return;
                }

                var userIdLong = Convert.ToInt64(userId);

                string userUrl;
                var embedBuilder = new EmbedBuilder();
                var user = DataManager.GetUserAsync(userIdLong).Result;
                if (user != null) {
                    userUrl = TwitchUrl + user.Login;
                    embedBuilder.WithAuthor(user.DisplayName, user.ProfileImageUrl, userUrl);
                } else {
                    DiscordStream.Instance.Logger.Warn("Failed to get User: {}", userId);
                    userUrl = TwitchUrl;
                    embedBuilder.WithAuthor("Unknown", null, userUrl);
                }

                if (streamData.Data.Count == 0) {
                    CurrentGames.Remove(userIdLong);
                    CurrentTitles.Remove(userIdLong);

                    embedBuilder.WithColor(MessageManager.GetColor(Color.Error));
                    embedBuilder.WithDescription("is now offline");
                    embedBuilder.WithTimestamp(DateTime.Now);
                    embedBuilder.WithFooter("Powered by " + Reference.Name);
                    MessageManager.SendMessageAsync(channel, embedBuilder.Build()).Wait();
                    return;
                }

                if (streamData.Data.Count > 1) {
                    DiscordStream.Instance.Logger.Warn("Multiple Streams found, This isn't supported");
                }

                var stream = streamData.Data[0];
                if (!string.Equals(stream.UserId, userId)) {
                    DiscordStream.Instance.Logger.Error("UserId mismatch!");
                    return;
                }

                var game = DataManager.GetGameAsync(Convert.ToInt64(stream.GameId)).Result;
                if (game == null) {
                    DiscordStream.Instance.Logger.Warn("Failed to get Game: {}", stream.GameId);
                }

                if (CurrentGames.ContainsKey(userIdLong) || CurrentTitles.ContainsKey(userIdLong)) {
                    if (!string.Equals(stream.GameId, CurrentGames.GetValueOrDefault(userIdLong, null))) {
                        CurrentGames[userIdLong] = stream.GameId;
                        if (game != null) {
                            embedBuilder.WithColor(MessageManager.GetColor(Color.Warning));
                            embedBuilder.WithDescription("is now playing [**" + game + "**](" + Toolbox.EncodeUrl(TwitchUrl + "directory/game/" + game) + ")");
                            embedBuilder.WithTimestamp(DateTime.Now);
                            embedBuilder.WithFooter("Powered by " + Reference.Name);
                            MessageManager.SendMessageAsync(channel, embedBuilder.Build()).Wait();
                        }
                    }

                    if (!string.Equals(stream.Title, CurrentTitles.GetValueOrDefault(userIdLong, null))) {
                        CurrentTitles[userIdLong] = stream.Title;
                        embedBuilder.WithColor(MessageManager.GetColor(Color.Warning));
                        embedBuilder.WithDescription("has changed their title to **" + stream.Title + "**");
                        embedBuilder.WithTimestamp(DateTime.Now);
                        embedBuilder.WithFooter("Powered by " + Reference.Name);
                        MessageManager.SendMessageAsync(channel, embedBuilder.Build()).Wait();
                    }

                    return;
                }

                CurrentGames.TryAdd(userIdLong, stream.GameId);
                CurrentTitles.TryAdd(userIdLong, stream.Title);

                embedBuilder.WithColor(MessageManager.GetColor(Color.Success));
                embedBuilder.WithDescription("is currently live streaming [**" + stream.Title + "**](" + userUrl + ")");
                embedBuilder.WithTimestamp(stream.StartedAt);
                embedBuilder.WithFooter("Powered by " + Reference.Name);
                if (game != null) {
                    embedBuilder.Description += " ([**" + game + "**](" + Toolbox.EncodeUrl(TwitchUrl + "directory/game/" + game) + "))";
                }

                MessageManager.SendMessageAsync(channel, embedBuilder.Build()).Wait();
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while processing stream", ex);
            }
        }
    }
}