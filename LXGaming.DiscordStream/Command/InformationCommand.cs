using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LXGaming.DiscordStream.Command.Attribute;
using LXGaming.DiscordStream.Manager;
using LXGaming.DiscordStream.Util;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using TwitchLib.Api;
using Color = LXGaming.DiscordStream.Entity.Color;

namespace LXGaming.DiscordStream.Command {

    public class InformationCommand : ModuleBase<SocketCommandContext> {

        [Command("information")]
        [Alias("info", "version")]
        [Summary("Displays bot information")]
        [RequirePermission("information.base")]
        public async Task ExecuteAsync() {
            var embedBuilder = new EmbedBuilder();
            embedBuilder.WithAuthor(DiscordStream.Name + " v" + DiscordStream.Version, AccountManager.DiscordClient.CurrentUser.GetAvatarUrl(), DiscordStream.Source);
            embedBuilder.WithColor(MessageManager.GetColor(Color.Default));
            embedBuilder.AddField("Uptime", Toolbox.GetTimeString((long) (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds));
            embedBuilder.AddField("Memory", GetHeapSize());
            embedBuilder.AddField("Runtime", ""
                                             + "\n- " + RuntimeInformation.FrameworkDescription
                                             + "\n- " + RuntimeInformation.OSDescription);
            embedBuilder.AddField("Dependencies", ""
                                                  + "\n- Discord.Net v" + GetVersion<DiscordConfig>()
                                                  + "\n- Microsoft.AspNetCore v" + GetVersion<IWebHost>()
                                                  + "\n- Newtonsoft.Json v" + GetVersion<JsonSerializer>()
                                                  + "\n- TwitchLib v" + GetVersion<TwitchAPI>());

            embedBuilder.WithFooter("Developed by " + DiscordStream.Authors);
            MessageManager.SendTemporaryMessageAsync(Context.Channel, embedBuilder.Build());

            await Task.CompletedTask;
        }

        private string GetHeapSize() {
            return Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString(CultureInfo.CurrentCulture) + " MB";
        }

        private string GetVersion<T>() {
            var assembly = typeof(T).GetTypeInfo().Assembly;

            var assemblyVersionAttribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            if (assemblyVersionAttribute != null) {
                return assemblyVersionAttribute.Version;
            }

            var version = assembly.GetName().Version?.ToString(3);
            if (!string.IsNullOrWhiteSpace(version)) {
                return version;
            }

            return "Unknown";
        }
    }
}