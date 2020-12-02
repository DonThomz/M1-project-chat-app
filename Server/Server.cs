using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private const int Port = 4000;

        private TcpListener _server;
        private static List<ClientManager> _clients = new List<ClientManager>();
        private bool _listening;

        public void Start()
        {
            try
            {
                Console.WriteLine("server chat-app start running...");
                _server = new TcpListener(IPAddress.Any, Port);
                _server.Start();
                RunServer();
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error during server setup : {0}", e.Message);
                Console.WriteLine("server chat-app stopping");
            }
        }

        private void RunServer()
        {
            _clients = new List<ClientManager>();
            _listening = true;

            Console.WriteLine("server chat-app listening on port {0}", Port.ToString());
            while (_listening)
            {
                Console.WriteLine("wait for clients...");
                // Wait util client connect to the server
                var client = _server.AcceptTcpClient();
                var clientManager = new ClientManager(client);
                
                // add method to remove client manager to the clients list
                clientManager.CloseConnectionEvent += RemoveClient;
                _clients.Add(clientManager);
                Console.WriteLine("client connected ! {0} connected", _clients.Count);
            }
        }

        private static void RemoveClient(object sender, ClientManager clientManager)
        {
            _clients.Remove(clientManager);
            Console.WriteLine("client {0} was removed", clientManager.RemotePort.ToString());
        }
    }
}