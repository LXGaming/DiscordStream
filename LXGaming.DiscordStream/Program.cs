using System;

namespace LXGaming.DiscordStream {

    public class Program {

        public static void Main(string[] args) {
            AppDomain.CurrentDomain.ProcessExit += Shutdown;

            var discordStream = new DiscordStream();
            discordStream.Load();
        }

        private static void Shutdown(object sender, EventArgs args) {
            DiscordStream.Instance?.State.Set();
        }
    }
}