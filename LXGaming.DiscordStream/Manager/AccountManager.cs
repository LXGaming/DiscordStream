using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LXGaming.DiscordStream.Command;
using LXGaming.DiscordStream.Listener;

namespace LXGaming.DiscordStream.Manager {

    public static class AccountManager {

        public static DiscordSocketClient DiscordClient { get; private set; }
        public static CommandService CommandService { get; private set; }

        public static void Prepare() {
            var accountCategory = DiscordStream.Instance.Config.AccountCategory;
            CreateDiscordClient(accountCategory.Token);
            Reload();
        }

        public static bool Reload() {
            var accountCategory = DiscordStream.Instance.Config?.AccountCategory;
            if (accountCategory == null) {
                DiscordStream.Instance.Logger.Warn("AccountCategory is unavailable");
                return false;
            }

            if (DiscordClient == null) {
                DiscordStream.Instance.Logger.Warn("DiscordClient is unavailable");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(accountCategory.ActivityName)) {
                DiscordClient.SetGameAsync(accountCategory.ActivityName, null, accountCategory.ActivityType);
            }

            DiscordClient.SetStatusAsync(accountCategory.UserStatus);
            return true;
        }

        public static void Shutdown() {
            DiscordClient?.Dispose();
        }

        private static async void CreateDiscordClient(string token) {
            try {
                if (string.IsNullOrWhiteSpace(token)) {
                    throw new ArgumentException("Token cannot be blank");
                }

                CommandService = new CommandService();
                DiscordClient = new DiscordSocketClient();
                DiscordClient.Log += LogAsync;
                DiscordClient.Ready += DiscordListener.ReadyAsync;
                DiscordClient.MessageReceived += DiscordListener.MessageReceivedAsync;

                await DiscordClient.LoginAsync(TokenType.Bot, token);
                await DiscordClient.StartAsync();
                await CommandService.AddModulesAsync(Assembly.GetAssembly(typeof(InfoCommand)), null);
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while creating DiscordClient", ex);
            }
        }

        private static Task LogAsync(LogMessage logMessage) {
            if (logMessage.Severity == LogSeverity.Info) {
                DiscordStream.Instance.Logger.Info(logMessage.Message, logMessage.Exception);
            } else if (logMessage.Severity == LogSeverity.Warning) {
                DiscordStream.Instance.Logger.Warn(logMessage.Message, logMessage.Exception);
            } else if (logMessage.Severity == LogSeverity.Error || logMessage.Severity == LogSeverity.Critical) {
                DiscordStream.Instance.Logger.Error(logMessage.Message, logMessage.Exception);
            } else {
                DiscordStream.Instance.Logger.Debug(logMessage.Message, logMessage.Exception);
            }

            return Task.CompletedTask;
        }
    }
}