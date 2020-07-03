using System.IO;
using System.Threading;
using LXGaming.DiscordStream.Configuration;
using LXGaming.DiscordStream.Integration.Twitch;
using LXGaming.DiscordStream.Manager;
using LXGaming.DiscordStream.Util;

namespace LXGaming.DiscordStream {

    public class DiscordStream {
        
        public const string Id = "discordstream";
        public const string Name = "DiscordStream";
        public const string Version = "1.1.0";
        public const string Authors = "LX_Gaming";
        public const string Source = "https://github.com/LXGaming/DiscordStream";
        public const string Website = "https://lxgaming.github.io/";
        
        // https://www.keycdn.com/blog/web-crawlers
        public const string UserAgent = Name + "/" + Version + "; (+" + Website + ")";

        public static DiscordStream Instance { get; private set; }
        public readonly ManualResetEvent State;
        public readonly Logger Logger;
        public readonly IConfiguration Configuration;

        public Config Config => Configuration.Config;

        public DiscordStream() {
            Instance = this;
            State = new ManualResetEvent(false);
            Logger = new Logger();
            Configuration = new JsonConfiguration(Directory.GetCurrentDirectory());
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

            Logger.Info("{} v{} has loaded", Name, Version);
            State.WaitOne();
        }

        public bool Reload() {
            Configuration.LoadConfiguration();
            if (Config == null) {
                return false;
            }

            Configuration.SaveConfiguration();
            ReloadLogger();

            return true;
        }

        public void ReloadLogger() {
            if (Config.GeneralCategory.Debug) {
                Logger.LoggerLevel = Logger.Level.Debug;
                Logger.Debug("Debug mode enabled");
            } else {
                Logger.LoggerLevel = Logger.Level.Info;
                Logger.Info("Debug mode disabled");
            }
        }
    }
}