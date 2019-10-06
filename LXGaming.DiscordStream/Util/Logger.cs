using System;
using System.Collections.Generic;

namespace LXGaming.DiscordStream.Util {

    public class Logger {

        private Level _loggerLevel = Level.Info;
        public Level LoggerLevel {
            get => _loggerLevel;
            set => _loggerLevel = value ?? Level.Off;
        }

        public void Debug(string format, params object[] arguments) {
            Log(Level.Debug, format, arguments);
        }

        public void Info(string format, params object[] arguments) {
            Log(Level.Info, format, arguments);
        }

        public void Warn(string format, params object[] arguments) {
            Log(Level.Warn, format, arguments);
        }

        public void Error(string format, params object[] arguments) {
            Log(Level.Error, format, arguments);
        }

        public void Log(Level level, string format, params object[] arguments) {
            if (LoggerLevel == Level.Off || LoggerLevel.Id < level.Id) {
                return;
            }

            Console.WriteLine(Format("[{}] [{}]: {}",
                DateTime.Now.ToString("HH:mm:ss"),
                level.ToString(),
                Format(format, arguments)));
        }

        private string Format(string format, params object[] arguments) {
            var exceptions = new List<Exception>();
            var index = 0;
            foreach (var argument in arguments) {
                if (index >= 0 && index < format.Length) {
                    index = format.IndexOf("{}", index, StringComparison.CurrentCulture);
                } else {
                    index = -1;
                }

                if (index == -1) {
                    if (argument is Exception exception) {
                        exceptions.Add(exception);
                    }

                    continue;
                }

                var length = format.Length;
                format = format.Remove(index, 2);
                format = format.Insert(index, GetString(argument));
                index += format.Length - length;
            }

            foreach (var exception in exceptions) {
                format += Environment.NewLine;
                format += exception.ToString();
                format += Environment.NewLine;
            }

            return format;
        }

        private string GetString(object argument) {
            if (argument == null) {
                return "null";
            }

            if (argument is Exception exception) {
                return exception.GetType().FullName + ": " + exception.Message;
            }

            return argument.ToString();
        }

        public class Level {

            public static readonly Level Off = new Level(0, "OFF");
            public static readonly Level Error = new Level(1, "ERROR");
            public static readonly Level Warn = new Level(2, "WARN");
            public static readonly Level Info = new Level(3, "INFO");
            public static readonly Level Debug = new Level(4, "DEBUG");
            public static readonly Level All = new Level(5, "INFO");

            public readonly int Id;
            public readonly string Name;

            private Level(int id, string name) {
                Id = id;
                Name = name;
            }

            public override string ToString() {
                return Name;
            }
        }
    }
}