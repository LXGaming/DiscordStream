using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LXGaming.DiscordStream.Command.Attribute;
using LXGaming.DiscordStream.Manager;
using Color = LXGaming.DiscordStream.Entity.Color;

namespace LXGaming.DiscordStream.Command {

    public class StreamCommand : ModuleBase<SocketCommandContext> {

        [Command("stream")]
        [Alias("streams")]
        [Summary("Displays streams we are watching")]
        [RequirePermission("stream.base")]
        public async Task ExecuteAsync() {
            var embedBuilder = new EmbedBuilder();
            embedBuilder.WithTitle("Streams");
            embedBuilder.WithColor(MessageManager.GetColor(Color.Default));
            foreach (var channel in DiscordStream.Instance.Config.IntegrationCategory.TwitchIntegrationCategory.Channels) {
                if (!string.IsNullOrEmpty(embedBuilder.Description)) {
                    embedBuilder.Description += "\n";
                }

                embedBuilder.Description += channel;
            }

            MessageManager.SendMessageAsync(Context.Channel, null, embedBuilder.Build());
            await Task.CompletedTask;
        }
    }
}