using System;
using System.IO;
using LXGaming.DiscordStream.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LXGaming.DiscordStream.Configuration {

    public class Configuration {

        private static readonly JsonSerializer JsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        });

        private readonly string _path;

        public Config Config { get; private set; }

        public Configuration(string path) {
            _path = path;
        }

        public bool LoadConfiguration() {
            var config = LoadFile<Config>(Path.Combine(_path, "config.json"));
            if (config == null) {
                return false;
            }

            Config = config;
            return true;
        }

        public bool SaveConfiguration() {
            return SaveFile(Path.Combine(_path, "config.json"), Config);
        }

        public static T LoadFile<T>(string path) {
            if (File.Exists(path)) {
                return DeserializeFile<T>(path);
            }

            var value = Toolbox.NewInstance<T>();
            if (value != null && SaveFile(path, value)) {
                return value;
            }

            return default;
        }

        public static bool SaveFile(string path, object value) {
            if (File.Exists(path) || CreateDirectory(path)) {
                return SerializeFile(path, value);
            }

            return false;
        }

        public static T DeserializeFile<T>(string path) {
            try {
                using (var streamReader = File.OpenText(path))
                using (var jsonTextReader = new JsonTextReader(streamReader)) {
                    return JsonSerializer.Deserialize<T>(jsonTextReader);
                }
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while deserializing {}", path, ex);
                return default;
            }
        }

        public static bool SerializeFile(string path, object value) {
            try {
                using (var streamWriter = File.CreateText(path)) {
                    JsonSerializer.Serialize(streamWriter, value);
                    return true;
                }
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while serializing {}", path, ex);
                return false;
            }
        }

        private static bool CreateDirectory(string path) {
            try {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                return true;
            } catch (Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while creating {}", path, ex);
                return false;
            }
        }
    }
}