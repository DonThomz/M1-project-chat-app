using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Server
    {
        private const int Port = 4000;
        private static string _consoleName;
        private static List<ClientManager> _clients = new List<ClientManager>();
        private bool _listening;
        private TcpListener _server;

        public Server(string consoleName)
        {
            _consoleName = $"{consoleName} >";
        }

        public void Start()
        {
            try
            {
                LogMessage($"{_consoleName} start running...");
                _server = new TcpListener(IPAddress.Any, Port);
                _server.Start();
                RunServer();
            }
            catch (SocketException e)
            {
                LogMessage($"{_consoleName} Error during server setup : {e.Message}");
                LogMessage($"{_consoleName} stopping");
            }
        }

        private void RunServer()
        {
            _clients = new List<ClientManager>();
            _listening = true;

            LogMessage($"{_consoleName} listening on port {Port.ToString()}");
            while (_listening)
            {
                LogMessage($"{_consoleName} wait for clients...");
                // Wait util client connect to the server
                var client = _server.AcceptTcpClient();
                var clientManager = new ClientManager(client);

                // add method to remove client manager to the clients list
                clientManager.CloseConnectionEvent += RemoveClient;
                _clients.Add(clientManager);
                LogMessage($"{_consoleName} client connected ! {_clients.Count.ToString()} connected");
            }
        }

        private static void RemoveClient(object sender, ClientManager clientManager)
        {
            _clients.Remove(clientManager);
            LogMessage($"{_consoleName} client {clientManager.RemotePort.ToString()} was removed");
        }

        private static void LogMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}