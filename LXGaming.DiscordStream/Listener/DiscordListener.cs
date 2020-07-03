using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LXGaming.DiscordStream.Manager;
using LXGaming.DiscordStream.Util;
using Color = LXGaming.DiscordStream.Entity.Color;

namespace LXGaming.DiscordStream.Listener {

    public static class DiscordListener {

        public static Task ReadyAsync() {
            DiscordStream.Instance.Logger.Info("{} connected", AccountManager.DiscordClient.CurrentUser);

            var accountCategory = DiscordStream.Instance.Config?.AccountCategory;
            if (accountCategory != null) {
                var id = AccountManager.DiscordClient.CurrentUser.Id;
                var name = AccountManager.DiscordClient.CurrentUser.Username;

                if (accountCategory.Id != id || !string.Equals(accountCategory.Name, name)) {
                    DiscordStream.Instance.Logger.Info("Account {} ({}) -> {} ({})", accountCategory.Name, accountCategory.Id, name, id);
                    accountCategory.Id = id;
                    accountCategory.Name = name;
                    DiscordStream.Instance.Configuration.SaveConfiguration();
                }
            }

            foreach (var guild in AccountManager.DiscordClient.Guilds) {
                GuildManager.Register(guild);
            }

            return Task.CompletedTask;
        }

        public static async Task MessageReceivedAsync(SocketMessage socketMessage) {
            var message = socketMessage as SocketUserMessage;
            if (message == null || message.Source != MessageSource.User) {
                return;
            }

            var config = DiscordStream.Instance.Config;
            if (config == null) {
                return;
            }

            var argumentPosition = 0;
            if (!message.HasStringPrefix(config.GeneralCategory.CommandPrefix, ref argumentPosition)) {
                return;
            }

            // Fixes issue where command is not found when the prefix and actual command are separated by a space
            if (message.Content.Length > argumentPosition && message.Content[argumentPosition] == ' ') {
                argumentPosition++;
            }

            if (config.MessageCategory.SendTyping) {
                await message.Channel.TriggerTypingAsync();
            }

            var context = new SocketCommandContext(AccountManager.DiscordClient, message);
            var result = await AccountManager.CommandService.ExecuteAsync(context, argumentPosition, null);
            if (result.IsSuccess) {
                if (config.MessageCategory.DeleteInvoking) {
                    await Task.Run(() => {
                        Task.Delay(TimeSpan.FromMilliseconds(config.MessageCategory.DeleteInterval))
                            .ContinueWith(task => message.DeleteAsync());
                    });
                }

                return;
            }

            var embedBuilder = new EmbedBuilder();
            embedBuilder.WithColor(MessageManager.GetColor(Color.Error));

            if (result is ExecuteResult executeResult) {
                if (executeResult.Exception is CommandException) {
                    embedBuilder.WithTitle("An error has occurred.");
                    embedBuilder.WithDescription("```" + executeResult.Exception.Message + "```");
                    embedBuilder.WithFooter("Exception: " + executeResult.Exception.GetType().FullName, "https://repo.lxgaming.me/assets/discord/error.png");
                    MessageManager.SendMessageAsync(context.Message.Channel, embedBuilder.Build());
                    return;
                }
            }

            DiscordStream.Instance.Logger.Error("Encountered an error while executing command: {}", result.ErrorReason);
            embedBuilder.WithTitle("An error has occurred. Details are available in console.");
            embedBuilder.WithFooter("Error: " + result.Error);
            MessageManager.SendMessageAsync(context.Message.Channel, embedBuilder.Build());
        }
    }
}