using System;
using System.Collections.Generic;
using LXGaming.DiscordStream.Configuration.Category;
using LXGaming.DiscordStream.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LXGaming.DiscordStream.Manager {

    public static class WebManager {

        public static readonly Guid Secret = Guid.NewGuid();

        public static IWebHost WebHost;

        public static void Prepare() {
            var webCategory = DiscordStream.Instance.Config.WebCategory;
            if (webCategory.ConnectTimeout < 0) {
                DiscordStream.Instance.Logger.Warn("ConnectTimeout is out of bounds. Resetting to {}",
                    WebCategory.DefaultConnectTimeout);
                webCategory.ConnectTimeout = WebCategory.DefaultConnectTimeout;
            }

            if (webCategory.ReadTimeout < 0) {
                DiscordStream.Instance.Logger.Warn("ReadTimeout is out of bounds. Resetting to {}",
                    WebCategory.DefaultReadTimeout);
                webCategory.ReadTimeout = WebCategory.DefaultReadTimeout;
            }

            if (webCategory.WriteTimeout < 0) {
                DiscordStream.Instance.Logger.Warn("WriteTimeout is out of bounds. Resetting to {}",
                    WebCategory.DefaultWriteTimeout);
                webCategory.WriteTimeout = WebCategory.DefaultWriteTimeout;
            }

            try {
                WebHost = new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureLogging(logging => {
                        logging.ClearProviders();
                        logging.AddConsole();
                    })
                    .ConfigureAppConfiguration(config => {
                        var dictionary = new Dictionary<string, string> {
                            {"Twitch:SecretKey", Secret.ToString()},

                            // TODO Add SSL support?
                            {"WebHooks:DisableHttpsCheck", "true"}
                        };

                        config.AddInMemoryCollection(dictionary);
                    })
                    .UseStartup<Startup>()
                    .UseUrls(webCategory.Url)
                    .Build();

                WebHost.RunAsync();
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Warn("Encountered an error while preparing WebHost", ex);
            }
        }
    }
}