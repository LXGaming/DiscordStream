using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using LXGaming.DiscordStream.Manager;

namespace LXGaming.DiscordStream.Command.Attribute {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : PreconditionAttribute {

        public string Permission { get; }

        public RequirePermissionAttribute(string permission) {
            Permission = permission;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services) {
            if (!(context.User is SocketGuildUser)) {
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command"));
            }

            var user = (SocketGuildUser) context.User;
            if (!string.IsNullOrWhiteSpace(Permission) && GuildManager.HasPermission(user, Permission)) {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError("You do not have permission to execute this command"));
        }
    }
}