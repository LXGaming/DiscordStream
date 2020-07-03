using System;
using LXGaming.DiscordStream.Manager;

namespace LXGaming.DiscordStream {

    public static class Program {

        public static void Main(string[] args) {
            AppDomain.CurrentDomain.ProcessExit += Shutdown;
            
            var discordStream = new DiscordStream();
            discordStream.Load();
        }

        private static void Shutdown(object sender, EventArgs args) {
            DiscordStream.Instance.Logger.Info("Shutting down...");
            DiscordStream.Instance.State.Set();
            
            AccountManager.Shutdown();
        }
    }
}