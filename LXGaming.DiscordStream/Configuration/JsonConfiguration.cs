using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LXGaming.DiscordStream.Configuration {

    public class JsonConfiguration : IConfiguration {

        private static readonly JsonSerializer JsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        });

        private readonly string _path;

        public Config Config { get; private set; }

        public JsonConfiguration(string path) {
            _path = path;
        }

        public bool LoadConfiguration() {
            var config = ((IConfiguration) this).LoadFile<Config>(Path.Combine(_path, "config.json"));
            if (config == null) {
                return false;
            }

            Config = config;
            return true;
        }

        public bool SaveConfiguration() {
            return ((IConfiguration) this).SaveFile(Path.Combine(_path, "config.json"), Config);
        }

        public T DeserializeFile<T>(string path) {
            try {
                using (var streamReader = File.OpenText(path))
                using (var jsonTextReader = new JsonTextReader(streamReader)) {
                    return JsonSerializer.Deserialize<T>(jsonTextReader);
                }
            } catch (System.Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while deserializing {}", path, ex);
                return default;
            }
        }

        public bool SerializeFile(string path, object value) {
            try {
                using (var streamWriter = File.CreateText(path)) {
                    JsonSerializer.Serialize(streamWriter, value);
                    return true;
                }
            } catch (System.Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while serializing {}", path, ex);
                return false;
            }
        }
    }
}