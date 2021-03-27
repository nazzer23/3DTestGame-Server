using System;
using System.Collections.Generic;
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
                case LogType.DEBUG:
                    // Disable if not in debug mode
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
        }
    }

    public enum LogType
    {
        DEBUG,
        SUCCESS,
        WARNING,
        ERROR
    }
}