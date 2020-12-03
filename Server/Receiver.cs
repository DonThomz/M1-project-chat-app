using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using Server.Manager;
using static ChatAppLib.models.communication.Command;

namespace Server
{
    public class Receiver
    {
        private static string _consoleName;
        public int RemotePort { get; }
        private State State { get; set; }
        private NetworkStream Stream { get; }
        private TcpClient Client { get; }

        private Queue<Response> ResponseQueue;

        /*
         * Managers
         */
        public MessageManager MessageManager { get; }

        /**
         * Threads
         */
        private Thread _receivingThread;

        private Thread _sendingThread;

        public event EventHandler<Receiver> CloseConnectionEvent;

        public Receiver(TcpClient client)
        {
            RemotePort = ((IPEndPoint) client.Client.RemoteEndPoint).Port;
            _consoleName = $"client {RemotePort.ToString()} >";

            State = State.CONNECTED;
            Client = client;
            Stream = client.GetStream();

            Client.ReceiveBufferSize = 1024;
            Client.SendBufferSize = 1024;

            MessageManager = new MessageManager();
            ResponseQueue = new Queue<Response>();
        }

        public void Start()
        {
            // launch the request listener 
            _receivingThread = new Thread(ListeningRequest);
            _receivingThread.IsBackground = true;
            _receivingThread.Start();

            // launch the response sender
            _sendingThread = new Thread(SendingResponse);
            _sendingThread.IsBackground = true;
            _sendingThread.Start();
        }

        private void AddResponseToQueue(object sender, Response response)
        {
            ResponseQueue.Enqueue(response);
        }

        /// <summary>
        /// Method handles request coming from client
        /// </summary>
        private void ListeningRequest()
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
                    DispatchRequest(requestReceive);
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

        /// <summary>
        /// Method to handle response that will be send to client
        /// </summary>
        private void SendingResponse()
        {
            MessageManager.SendResponseMessageEvent += AddResponseToQueue;
            while (State != State.DISCONNECTED)
            {
                if (ResponseQueue.Count > 0)
                {
                    try
                    {
                        // get the response and remove it to the queue
                        var response = ResponseQueue.Dequeue();
                        var serializeRequest = Response.Serialize(response);
                        Stream.Write(serializeRequest, 0, serializeRequest.Length);
                        Stream.Flush();
                        LogMessage($"response send to the user {RemotePort.ToString()}");
                    }
                    catch (Exception e)
                    {
                        LogMessage($"Error sending message {e.Message}");
                        State = State.DISCONNECTED;
                    }
                }

                Thread.Sleep(30);
            }
        }

        private void DispatchRequest(Request request)
        {
            switch (request.Type)
            {
                case PRIVATE_MESSAGE:
                    MessageManager.HandlePrivateMessageSend(request);
                    break;
                default:
                    break;
            }
        }

        private static void LogMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0} {1}", _consoleName, message);
        }
    }
}