using System;
using System.Collections.Generic;

namespace LXGaming.DiscordStream.Util {

    public class Logger {

        public bool ConsoleColors { get; set; } = true;
        public Level LoggerLevel { get; set; } = Level.Info;
        
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
            if (LoggerLevel == null || LoggerLevel == Level.Off || LoggerLevel.Id < level.Id) {
                return;
            }
            
            if (ConsoleColors && level.ForegroundColor.HasValue) {
                Console.ForegroundColor = level.ForegroundColor.Value;
            }
            
            Console.WriteLine(Format("[{}] [{}]: {}",
                DateTime.Now.ToString("HH:mm:ss"),
                level.ToString(),
                Format(format, arguments)));
            
            if (ConsoleColors && level.ForegroundColor.HasValue) {
                Console.ResetColor();
            }
        }

        private string Format(string format, params object[] arguments) {
            var exceptions = new List<System.Exception>();
            var index = 0;
            foreach (var argument in arguments) {
                if (index >= 0 && index < format.Length) {
                    index = format.IndexOf("{}", index, StringComparison.CurrentCulture);
                } else {
                    index = -1;
                }

                if (index == -1) {
                    if (argument is System.Exception exception) {
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
            if (argument is System.Exception exception) {
                return exception.GetType().FullName + ": " + exception.Message;
            }

            return argument?.ToString() ?? "null";
        }

        public class Level {

            public static readonly Level Off = new Level(0, "OFF");
            public static readonly Level Error = new Level(1, "ERROR", ConsoleColor.Red);
            public static readonly Level Warn = new Level(2, "WARN", ConsoleColor.Yellow);
            public static readonly Level Info = new Level(3, "INFO");
            public static readonly Level Debug = new Level(4, "DEBUG", ConsoleColor.Cyan);
            public static readonly Level All = new Level(5, "ALL");

            public readonly int Id;
            public readonly string Name;
            public readonly ConsoleColor? ForegroundColor;

            private Level(int id, string name, ConsoleColor? foregroundColor = null) {
                Id = id;
                Name = name;
                ForegroundColor = foregroundColor;
            }
            
            public override string ToString() {
                return Name;
            }
        }
    }
}