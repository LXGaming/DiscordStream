using System.IO;
using System.Threading;
using LXGaming.DiscordStream.Configuration;
using LXGaming.DiscordStream.Integration.Twitch;
using LXGaming.DiscordStream.Manager;
using LXGaming.DiscordStream.Util;

namespace LXGaming.DiscordStream {

    public class DiscordStream {

        public static DiscordStream Instance { get; private set; }
        public readonly ManualResetEvent State;
        public readonly Logger Logger;
        public readonly Configuration.Configuration Configuration;

        public Config Config => Configuration.Config;

        public DiscordStream() {
            Instance = this;
            State = new ManualResetEvent(false);
            Logger = new Logger();
            Configuration = new Configuration.Configuration(Directory.GetCurrentDirectory());
        }

        public void Load() {
            Logger.Info("Initializing...");
            if (!Reload()) {
                Logger.Error("Failed to load");
                return;
            }

            AccountManager.Prepare();
            MessageManager.Prepare();
            WebManager.Prepare();

            TwitchIntegration.Prepare();

            Configuration.SaveConfiguration();

            Logger.Info("{} v{} has loaded", Reference.Name, Reference.Version);
            State.WaitOne();
        }

        public bool Reload() {
            Configuration.LoadConfiguration();
            if (Config == null) {
                return false;
            }

            Configuration.SaveConfiguration();
            if (Config.GeneralCategory.Debug) {
                Logger.LoggerLevel = Logger.Level.Debug;
                Logger.Debug("Debug mode enabled");
            } else {
                Logger.LoggerLevel = Logger.Level.Info;
                Logger.Info("Debug mode disabled");
            }

            return true;
        }
    }
}