using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class Server
    {
        private const int Port = 4000;
        private static string _consoleName;
        public static readonly List<Receiver> Clients = new List<Receiver>();
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
                WaitForClients();
            }
            catch (SocketException e)
            {
                LogMessage($"{_consoleName} Error during server setup : {e.Message}");
                LogMessage($"{_consoleName} stopping");
            }
        }

        private void WaitForClients()
        {
            _listening = true;

            LogMessage($"{_consoleName} listening on port {Port.ToString()}");
            while (_listening)
            {
                LogMessage($"{_consoleName} wait for clients...");
                // Wait util client connect to the server
                var client = _server.AcceptTcpClient();
                var clientManager = new Receiver(client);

                // add method to remove client manager to the clients list
                clientManager.CloseConnectionEvent += RemoveClient;
                Clients.Add(clientManager);

                // Start the client managing in a thread
                var clientThread = new Thread(clientManager.Start);
                clientThread.Start();

                LogMessage($"{_consoleName} client connected ! {Clients.Count.ToString()} connected");
            }
        }

        private static void RemoveClient(object sender, Receiver clientManager)
        {
            Clients.Remove(clientManager);
            LogMessage($"{_consoleName} client {clientManager.RemotePort.ToString()} was removed");
        }

        private static void LogMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}