using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LXGaming.DiscordStream.Command.Attribute;
using LXGaming.DiscordStream.Manager;
using Color = LXGaming.DiscordStream.Entity.Color;
using CommandException = LXGaming.DiscordStream.Exception.CommandException;

namespace LXGaming.DiscordStream.Command {

    public class DebugCommand : ModuleBase<SocketCommandContext> {

        [Command("debug")]
        [RequirePermission("debug.base")]
        public async Task ExecuteAsync(bool? state = null) {
            var generalCategory = DiscordStream.Instance.Config?.GeneralCategory;
            if (generalCategory == null) {
                throw new CommandException("GeneralCategory is unavailable");
            }

            if (state.HasValue) {
                generalCategory.Debug = state.Value;
            } else {
                generalCategory.Debug = !generalCategory.Debug;
            }

            DiscordStream.Instance.Configuration.SaveConfiguration();
            DiscordStream.Instance.ReloadLogger();

            var embedBuilder = new EmbedBuilder();
            if (generalCategory.Debug) {
                embedBuilder.WithColor(MessageManager.GetColor(Color.Success));
                embedBuilder.WithTitle("Debugging enabled");
            } else {
                embedBuilder.WithColor(MessageManager.GetColor(Color.Warning));
                embedBuilder.WithTitle("Debugging disabled");
            }

            MessageManager.SendTemporaryMessageAsync(Context.Channel, embedBuilder.Build());
            await Task.CompletedTask;
        }
    }
}