using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;

namespace Rocket.Core.Logging
{
    public partial class Logger
    {
        [Obsolete("Log(string message,bool sendToConsole) is obsolete, use Log(string message,ConsoleColor color) instead", true)]
        public static void Log(string message, bool sendToConsole) => PointBlankLogging.Log(message, sendToConsole);
        public static void Log(string message, ConsoleColor color = ConsoleColor.White) => PointBlankLogging.Log(message, true, color);
        public static void Log(Exception ex) => LogException(ex);

        public static void LogWarning(string message) => PointBlankLogging.LogWarning(message);

        public static void LogError(string message) => PointBlankLogging.Log(message, true, ConsoleColor.Red);
        public static void LogError(Exception ex, string v) => LogException(ex, v);

        public static void LogException(Exception ex, string message = null) => PointBlankLogging.LogError((string.IsNullOrEmpty(message) ? "?" : message), ex);
    }
}
