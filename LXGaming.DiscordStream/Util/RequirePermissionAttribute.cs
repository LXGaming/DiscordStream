using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using LXGaming.DiscordStream.Manager;

namespace LXGaming.DiscordStream.Util {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequirePermissionAttribute : PreconditionAttribute {

        public string Permission { get; }

        public RequirePermissionAttribute(string permission) {
            Permission = permission;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services) {
            if (context.User is SocketGuildUser user) {
                if (!string.IsNullOrWhiteSpace(Permission) && PermissionManager.HasPermission(user, Permission)) {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }

                return Task.FromResult(PreconditionResult.FromError("You do not have permission to execute this command"));
            }

            return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command"));
        }
    }
}