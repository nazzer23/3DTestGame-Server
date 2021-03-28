using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Game.Handlers;

namespace Game
{
    public class Core
    {

        private static Core _instance = null;
        public static readonly string programStartTime = DateTime.Now.ToFileTime().ToString();
        public static bool isRunning = true;
        private TcpListener socketTcpListener = null;

        public static Core GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Core();
            }
            return _instance;
        }

        public void Init()
        {
            if (LogHandler.GetInstance() != null)
            {
                LogHandler.GetInstance().Log("LogHandler has initialized", LogType.SUCCESS);
            }
            else
            {
                this.Error("There was an error whilst initializing the LogHandler.");
            }

            if (ConfigHandler.GetInstance().Initialize())
            {
                LogHandler.GetInstance().Log("ConfigHandler has initialized", LogType.SUCCESS);
            }
            else
            {
                this.Error("There was an error whilst initializing the ConfigHandler.");
            }

            if (DatabaseHandler.GetInstance().Initialize())
            {
                LogHandler.GetInstance().Log("DatabaseHandler has initialized", LogType.SUCCESS);
            }
            else
            {
                this.Error("There was an error whilst initializing the DatabaseHandler");
            }


        }

        public void Error(string message)
        {
            LogHandler.GetInstance().Log(message, LogType.ERROR);
            this.Shutdown();
        }

        public void Shutdown()
        {
            // Shutdown procedures happen here - tcp close, database end, etc
            DatabaseHandler.GetInstance().Shutdown();
            Core.isRunning = false;
        }

    }
}