using System.Collections.Generic;
using System.Linq;
using ConcurrentCollections;
using Discord;
using LXGaming.DiscordStream.Configuration.Category;
using LXGaming.DiscordStream.Configuration.Category.Guild;

namespace LXGaming.DiscordStream.Manager {

    public class GuildManager {

        public static void Register(IGuild guild) {
            var roleCategories = new ConcurrentHashSet<RoleCategory>();
            foreach (var role in guild.Roles) {
                var roleCategory = GetRoleCategory(guild.Id, role.Id) ?? new RoleCategory();
                roleCategory.Id = role.Id;
                roleCategory.Name = role.Name;
                roleCategories.Add(roleCategory);
            }

            var guildCategory = GetGuildCategory(guild.Id) ?? new GuildCategory();
            guildCategory.Id = guild.Id;
            guildCategory.Name = guild.Name;
            guildCategory.RoleCategories = roleCategories;

            DiscordStream.Instance.Config?.GuildCategories.Add(guildCategory);
            DiscordStream.Instance.Configuration.SaveConfiguration();
        }

        public static bool HasPermission(IGuildUser user, string permission) {
            var permissions = GetPermissions(user);
            if (permissions.Count == 0 || permissions.Contains("!" + permission)) {
                return false;
            }

            return permissions.Contains(permission) || permissions.Contains("*");
        }

        public static HashSet<string> GetPermissions(IGuildUser user) {
            var permissions = new HashSet<string>();

            var userCategory = GuildManager.GetUserCategory(user);
            if (userCategory != null) {
                AppendPermissions(permissions, userCategory.Permissions);
            }

            foreach (var roleCategory in GuildManager.GetRoleCategories(user)) {
                AppendPermissions(permissions, roleCategory.Permissions);
            }

            return permissions;
        }

        private static void AppendPermissions(ICollection<string> collection, IEnumerable<string> enumerable) {
            foreach (var permission in enumerable) {
                AppendPermission(collection, permission);
            }
        }

        private static void AppendPermission(ICollection<string> permissions, string permission) {
            if (permission.StartsWith("!") && permissions.Contains(permission.Substring(1))) {
                return;
            }

            permissions.Add(permission);
        }

        public static List<RoleCategory> GetRoleCategories(IGuildUser user) {
            var roleCategories = new List<RoleCategory>();

            foreach (var role in user.Guild.Roles) {
                if (user.RoleIds.Contains(role.Id)) {
                    var roleCategory = GetRoleCategory(user.GuildId, role.Id);
                    if (roleCategory != null) {
                        roleCategories.Add(roleCategory);
                    }

                    continue;
                }

                if (roleCategories.Count != 0) {
                    var roleCategory = GetRoleCategory(user.GuildId, role.Id);
                    if (roleCategory != null && roleCategory.Inheritable) {
                        roleCategories.Add(roleCategory);
                    }
                }
            }

            // TODO Check the public role is included in roleIds
            return roleCategories;
        }

        public static RoleCategory GetRoleCategory(ulong guildId, ulong roleId) {
            var guildCategory = GetGuildCategory(guildId);
            if (guildCategory == null) {
                return null;
            }

            foreach (var roleCategory in guildCategory.RoleCategories) {
                if (roleCategory.Id == roleId) {
                    return roleCategory;
                }
            }

            return null;
        }

        public static UserCategory GetOrCreateUserCategory(IGuildUser user) {
            var guildCategory = GetGuildCategory(user.GuildId);
            if (guildCategory == null) {
                return null;
            }

            foreach (var userCategory in guildCategory.UserCategories) {
                if (userCategory.Id == user.Id) {
                    return userCategory;
                }
            }

            var newUserCategory = new UserCategory {
                Id = user.Id,
                Name = user.Username
            };

            guildCategory.UserCategories.Add(newUserCategory);
            return newUserCategory;
        }

        public static UserCategory GetUserCategory(IGuildUser user) {
            var guildCategory = GetGuildCategory(user.GuildId);
            if (guildCategory == null) {
                return null;
            }

            foreach (var userCategory in guildCategory.UserCategories) {
                if (userCategory.Id == user.Id) {
                    return userCategory;
                }
            }

            return null;
        }

        public static GuildCategory GetGuildCategory(ulong guildId) {
            var guildCategories = DiscordStream.Instance.Config?.GuildCategories;
            if (guildCategories == null) {
                return null;
            }

            foreach (var guildCategory in guildCategories) {
                if (guildCategory.Id == guildId) {
                    return guildCategory;
                }
            }

            return null;
        }
    }
}