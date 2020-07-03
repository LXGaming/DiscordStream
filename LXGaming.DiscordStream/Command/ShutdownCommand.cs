using System.Threading.Tasks;
using Discord.Commands;
using LXGaming.DiscordStream.Command.Attribute;
using LXGaming.DiscordStream.Util;

namespace LXGaming.DiscordStream.Command {

    public class ShutdownCommand : ModuleBase<SocketCommandContext> {

        [Command("shutdown")]
        [Summary("Shuts down the bot")]
        [RequirePermission("shutdown.base")]
        public async Task ExecuteAsync() {
            DiscordStream.Instance.State.Set();
            await Context.Message.DeleteAsync();
        }
    }
}