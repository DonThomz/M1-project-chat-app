using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChatAppLib;
namespace Server
{
    public class ClientManager
    {
        private static int ID = 0;
        private readonly int _remotePort;
        private readonly User _user;
        private State _state;
        private readonly NetworkStream _stream;
        private readonly TcpClient _client;

        public event EventHandler<ClientManager> CloseConnectionEvent; 
        
        public int RemotePort => _remotePort;

        public User User => _user;

        public State State => _state;

        public NetworkStream Stream => _stream;

        public TcpClient Client => _client;

        public ClientManager(TcpClient client)
        {
            _remotePort = ((IPEndPoint) client.Client.RemoteEndPoint).Port;
            ID++;
            _state = State.CONNECTED;
            _client = client;
            _stream = client.GetStream();
            
            
            var clientThread = new Thread(Listen);
            clientThread.Start();
        }

        private void Listen()
        {
            var buffer = new byte[1024];
            
            Console.WriteLine("Start listen to client n°{0} request ...", _remotePort.ToString());
            while (this._state != State.DISCONNECTED)
            {
                Console.WriteLine("reading...");
                try
                {
                    var byteCount = _stream.Read(buffer, 0, buffer.Length);
                    var formatted = new byte[byteCount];

                    // if not byte read
                    if (byteCount == 0) break;

                    var data = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Console.WriteLine(data);
                }
                catch (Exception ignored)
                {
                    Console.WriteLine("connection lost with the client {0}", _remotePort.ToString());
                    _state = State.DISCONNECTED;
                    // invoke event to remove client manager from the list 
                    CloseConnectionEvent?.Invoke(this, this);
                }
            }
        }
    }
}