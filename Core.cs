using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public bool canListen = true;

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

            if (DatabaseHandler.GetInstance().GetNumberOfRows("SELECT * FROM servers WHERE Name LIKE '" + ConfigHandler.GetInstance().GetString("serverName") + "'") <= 0)
            {
                this.Error("Unable to find server data for the entered server name.");
            }

            var serverPort = DatabaseHandler.GetInstance().Fetch($"SELECT * FROM servers WHERE Name LIKE '{ConfigHandler.GetInstance().GetString("serverName")}'").Rows[0]["Port"];
            LogHandler.GetInstance().Log("Starting server on port " + serverPort, LogType.INFO);
            Listen((Int32)serverPort);
        }

        private void Listen(int port)
        {
            this.socketTcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            this.socketTcpListener.Start();
            LogHandler.GetInstance().Log($"Listening on {this.socketTcpListener.LocalEndpoint}", LogType.SUCCESS);
            int counter = 0;
            while (this.socketTcpListener.Server.IsBound && this.canListen)
            {
                if (this.socketTcpListener.Pending())
                {
                    TcpClient client = this.socketTcpListener.AcceptTcpClient();
                    LogHandler.GetInstance().Log($"Accepting client {counter}", LogType.INFO);

                    //byte[] writeBytes = Encoding.ASCII.GetBytes("OMG, MAPRIL IT WORKS");

                    //NetworkStream nw = client.GetStream();
                    //nw.Write(writeBytes, 0, writeBytes.Length);
                    //nw.Flush();

                    counter++;
                }
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