using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LXGaming.DiscordStream.Integration.Twitch;
using LXGaming.DiscordStream.Util;
using TwitchLib.Api.Helix.Models.Users;

namespace LXGaming.DiscordStream.Manager {

    public static class DataManager {

        public static readonly Dictionary<long, string> TwitchGameCache = new Dictionary<long, string>();
        public static readonly Dictionary<long, User> TwitchUserCache = new Dictionary<long, User>();

        public static string GetCachedGame(long gameId) {
            return TwitchGameCache.GetValueOrDefault(gameId, null);
        }

        public static async Task<string> GetGameAsync(long gameId) {
            var cachedGame = GetCachedGame(gameId);
            if (cachedGame != null) {
                return cachedGame;
            }

            var games = await TwitchIntegration.TwitchApi.Helix.Games.GetGamesAsync(new List<string>() {gameId.ToString()});
            if (games == null) {
                return null;
            }

            foreach (var game in games.Games) {
                Toolbox.Put(TwitchGameCache, Convert.ToInt64(game.Id), game.Name);
            }

            return GetCachedGame(gameId);
        }

        public static User GetCachedUser(long userId) {
            return TwitchUserCache.GetValueOrDefault(userId, null);
        }

        public static async Task<User> GetUserAsync(long userId) {
            var cachedUser = GetCachedUser(userId);
            if (cachedUser != null) {
                return cachedUser;
            }

            var users = await TwitchIntegration.TwitchApi.Helix.Users.GetUsersAsync(new List<string>() {userId.ToString()});
            if (users == null) {
                return null;
            }

            foreach (var user in users.Users) {
                Toolbox.Put(TwitchUserCache, Convert.ToInt64(user.Id), user);
            }

            return GetCachedUser(userId);
        }
    }
}