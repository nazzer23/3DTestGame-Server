using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Handlers
{
    public class ClientHandler
    {
        private TcpClient tcpClient;
        public NetworkStream NetworkStream;

        public ClientHandler(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.NetworkStream = tcpClient.GetStream();
        }

        public void Start()
        {
            Core.GetInstance().SendPacket("Client successfully handled.", this);
            this.ReadData();
            this.Disconnect();
        }

        public void ReadData()
        {
            while (tcpClient.Connected)
            {
                LogHandler.GetInstance().Log($"Connected Clients: {Core.GetInstance().connectedClients.Count}", LogType.INFO);
            }
        }

        public void Disconnect()
        {
            Core.GetInstance().connectedClients.Remove(this);
        }
    }
}