using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public static class Logger
    {
        public static LogLevel Level { get; set; } = LogLevel.Information;

        public enum LogLevel
        {
            Error,
            Information,
            Debug
        }

        public class LoggerMessageEventArgs
        {
            public LogLevel Level = LogLevel.Debug;
            public string Message = "";
        }

        public static event EventHandler<LoggerMessageEventArgs> MessageLogged = delegate { };

        private static void LogMessage(string message, LogLevel level)
        {
            if (level <= Level)
            {
                LoggerMessageEventArgs eventArgs = new LoggerMessageEventArgs();
                eventArgs.Message = message;
                eventArgs.Level = level;
                MessageLogged?.Invoke(null, eventArgs);
            }
        }

        public static void WriteLine(string message, LogLevel level)
        {
            LogMessage(message + Environment.NewLine, level);
        }


    }
}
