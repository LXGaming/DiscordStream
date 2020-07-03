using System;
using System.IO;

namespace LXGaming.DiscordStream.Configuration {

    public interface IConfiguration {

        Config Config { get; }

        bool LoadConfiguration();

        bool SaveConfiguration();

        T LoadFile<T>(string path) {
            if (File.Exists(path)) {
                return DeserializeFile<T>(path);
            }

            var value = Activator.CreateInstance<T>();
            if (SaveFile(path, value)) {
                return value;
            }

            return default;
        }

        bool SaveFile(string path, object value) {
            if (File.Exists(path) || CreateDirectory(path)) {
                return SerializeFile(path, value);
            }

            return false;
        }

        T DeserializeFile<T>(string path);

        bool SerializeFile(string path, object value);

        static bool CreateDirectory(string path) {
            try {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                return true;
            } catch (System.Exception ex) {
                DiscordStream.Instance.Logger.Error("Encountered an error while creating {}", path, ex);
                return false;
            }
        }
    }
}