using System;
using System.Threading.Tasks;
using Discord;
using LXGaming.DiscordStream.Util;
using Color = LXGaming.DiscordStream.Data.Color;

namespace LXGaming.DiscordStream.Manager {

    public static class MessageManager {

        public const string DefaultColor = "7289DA"; // Blurple

        public static void Prepare() {
            var messageCategory = DiscordStream.Instance.Config.MessageCategory;
            messageCategory.Colors.TryAdd(Color.Error, "C13737");
            messageCategory.Colors.TryAdd(Color.Success, "46A84B");
            messageCategory.Colors.TryAdd(Color.Warning, "EAA245");

            foreach (Color color in Enum.GetValues(typeof(Color))) {
                messageCategory.Colors.TryAdd(color, DefaultColor);
            }
        }

        public static async Task SendTemporaryMessageAsync(IMessageChannel channel, string text) {
            await SendTemporaryMessageAsync(channel, text, null);
        }

        public static async Task SendTemporaryMessageAsync(IMessageChannel channel, Embed embed) {
            await SendTemporaryMessageAsync(channel, null, embed);
        }

        public static async Task SendTemporaryMessageAsync(IMessageChannel channel, string text, Embed embed) {
            try {
                var message = await channel.SendMessageAsync(text, false, embed);
                var messageCategory = DiscordStream.Instance.Config?.MessageCategory;
                if (messageCategory == null || !messageCategory.DeleteMessages || messageCategory.DeleteInterval <= 0) {
                    return;
                }

                // TODO Figure out why this is blocking...
                await Task.Delay(TimeSpan.FromMilliseconds(messageCategory.DeleteInterval))
                    .ContinueWith(task => message.DeleteAsync());
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while sending message", ex);
            }
        }

        public static async Task SendMessageAsync(IMessageChannel channel, string text) {
            await SendMessageAsync(channel, text, null);
        }

        public static async Task SendMessageAsync(IMessageChannel channel, Embed embed) {
            await SendMessageAsync(channel, null, embed);
        }

        public static async Task SendMessageAsync(IMessageChannel channel, string text, Embed embed) {
            try {
                await channel.SendMessageAsync(text, false, embed);
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while sending message", ex);
            }
        }

        public static Discord.Color GetColor(Color color) {
            var hex = DiscordStream.Instance.Config?.MessageCategory.Colors[color]?.TrimStart('#');
            if (hex == null) {
                return Toolbox.ParseDiscordColor(DefaultColor);
            } else if (hex.Length == 6) {
                return Toolbox.ParseDiscordColor(hex);
            } else {
                DiscordStream.Instance.Logger.Warn("Invalid Color: {} ({})", hex, Enum.GetName(typeof(Color), color));
                return Toolbox.ParseDiscordColor(DefaultColor);
            }
        }
    }
}