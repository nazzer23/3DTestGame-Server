using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class LogHandler
    {
        private static LogHandler _instance = null;

        public static LogHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LogHandler();
            }

            return _instance;
        }

        public void Log(string message, LogType type)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            switch (type)
            {
                case LogType.INFO:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogType.DEBUG:
                    // Disable if not in debug mode
                    if (!ConfigHandler.GetInstance().GetBool("debugMode"))
                        return;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogType.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.ERROR:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
            Console.Write("[{0}]", type); // Set Color of the Prefix
            Console.ForegroundColor = ConsoleColor.Gray;
            const string outputFormat = "[{0}] {1}";
            Console.WriteLine(outputFormat, DateTime.Now, message);

            LogToFile("[{0}] [{1}] {2}", type, DateTime.Now, message);
        }

        private static void LogToFile(string format, params object[] a)
        {
            string logFile = "logs/log-" + Core.programStartTime + ".txt";
            if (!Directory.Exists(@"logs"))
            {
                Directory.CreateDirectory(@"logs");
            }

            if (!File.Exists(logFile))
            {
                File.CreateText(logFile).Close();
            }

            var streamWriter = new StreamWriter(logFile, true);
            streamWriter.WriteLine(format, a);
            streamWriter.Close();
        }
    }

    public enum LogType
    {
        DEBUG,
        SUCCESS,
        WARNING,
        ERROR,
        INFO
    }
}