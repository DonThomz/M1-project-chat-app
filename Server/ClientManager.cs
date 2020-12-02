using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Server
{
    public class ClientManager
    {
        private static string _consoleName;

        public ClientManager(TcpClient client)
        {
            RemotePort = ((IPEndPoint) client.Client.RemoteEndPoint).Port;
            _consoleName = $"client {RemotePort.ToString()} >";

            State = State.CONNECTED;
            Client = client;
            Stream = client.GetStream();

            Client.ReceiveBufferSize = 1024;

            var clientThread = new Thread(ListenResponse);
            clientThread.Start();
        }

        public int RemotePort { get; }

        public User User { get; }

        public State State { get; private set; }

        public NetworkStream Stream { get; }

        public TcpClient Client { get; }

        public event EventHandler<ClientManager> CloseConnectionEvent;

        private void ListenResponse()
        {
            LogMessage("start listen to client request ...");
            while (State != State.DISCONNECTED)
            {
                LogMessage("reading...");
                try
                {
                    var formatter = new BinaryFormatter();
                    var requestReceive = (Request) formatter.Deserialize(Stream);
                    LogMessage(requestReceive);
                }
                catch (SerializationException serializationException)
                {
                    LogMessage($"error during deserialization : {serializationException.Message}");
                }
                catch (Exception e)
                {
                    LogMessage("connection lost, removing client...");
                    State = State.DISCONNECTED;
                    // invoke event to remove client manager from the list 
                    CloseConnectionEvent?.Invoke(this, this);
                }
            }
        }

        private static void LogMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0} {1}", _consoleName, message);
        }
    }
}