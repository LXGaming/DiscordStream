using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;

namespace LXGaming.DiscordStream.Util {

    public static class Toolbox {

        public const string ZeroWidthSpace = "\u200E";

        public static string Filter(string input) {
            return Regex.Replace(input, "[^\\x20-\\x7E\\x0A\\x0D]", "");
        }

        public static string FilterDiscord(string input) {
            return input
                .Replace("\\", "\\\\")
                .Replace("*", "\\*") // Bold
                .Replace("_", "\\_") // Underline
                .Replace("~", "\\~") // Strikethrough
                .Replace("`", "\\`") // Code Blocks
                .Replace(">", "\\>") // Block Quotes
                .Replace("[", "\\[").Replace("]", "\\]")
                .Replace("(", "\\(").Replace(")", "\\)");
        }

        public static string EncodeUrl(string input) {
            return input.Replace(" ", "%20");
        }

        public static Color ParseDiscordColor(string color) {
            return new Color(uint.Parse(color, NumberStyles.HexNumber));
        }

        public static IMessageChannel GetTextChannel(DiscordSocketClient discordClient, ulong channelId) {
            foreach (var guild in discordClient.Guilds) {
                foreach (var textChannel in guild.TextChannels) {
                    if (textChannel.Id == channelId) {
                        return textChannel;
                    }
                }
            }

            return null;
        }

        public static string GetTimeString(long millisecond) {
            var second = Math.Abs(millisecond) / 1000;
            var minute = second / 60;
            var hour = minute / 60;
            var day = hour / 24;

            var stringBuilder = new StringBuilder();
            AppendUnit(stringBuilder, day, "day", "days");
            AppendUnit(stringBuilder, hour % 24, "hour", "hours");
            AppendUnit(stringBuilder, minute % 60, "minute", "minutes");
            AppendUnit(stringBuilder, second % 60, "second", "seconds");

            if (stringBuilder.Length == 0) {
                stringBuilder.Append("just now");
            }

            return stringBuilder.ToString();
        }

        public static void AppendUnit(StringBuilder stringBuilder, long unit, string singular, string plural) {
            if (unit > 0) {
                if (stringBuilder.Length > 0) {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(unit).Append(" ").Append(FormatUnit(unit, singular, plural));
            }
        }

        public static string FormatUnit(long unit, string singular, string plural) {
            if (unit == 1) {
                return singular;
            }

            return plural;
        }

        public static TValue Put<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value) {
            var previousValue = dictionary.GetValueOrDefault(key);
            dictionary.Remove(key);
            dictionary.Add(key, value);
            return previousValue;
        }

        public static T NewInstance<T>() {
            return NewInstance<T>(typeof(T));
        }

        public static T NewInstance<T>(Type type) {
            try {
                return (T) Activator.CreateInstance(type);
            } catch (Exception) {
                return default;
            }
        }
    }
}