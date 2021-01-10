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
        private readonly string _consoleName;

        private readonly Queue<Response> ResponseQueue;

        /**
         * Threads
         */
        private Thread _receivingThread;

        private Thread _sendingThread;

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
            TopicManager = new TopicManager();
            AuthManager = new AuthManager();
            ResponseQueue = new Queue<Response>();
        }

        public int RemotePort { get; }
        private State State { get; set; }
        private NetworkStream Stream { get; }
        private TcpClient Client { get; }
        
        /*
         * Managers
         */
        public MessageManager MessageManager { get; }
        public TopicManager TopicManager { get; }

        public AuthManager AuthManager { get; }
        
        
        public event EventHandler<Receiver> CloseConnectionEvent;

        public void Start()
        {
            // add events
            MessageManager.SendResponseMessageEvent += AddResponseToQueue;
            TopicManager.SendResponseTopicEvent += AddResponseToQueue;
            AuthManager.SendResponseAuthEvent += AddResponseToQueue;

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
        ///     Method handles request coming from client
        /// </summary>
        private void ListeningRequest()
        {
            LogMessage("start listen to client request ...");
            var formatter = new BinaryFormatter();
            while (State != State.DISCONNECTED)
                try
                {
                    var requestReceive = (Request) formatter.Deserialize(Stream);
                    LogMessage($"Request receive : {requestReceive.Type}");
                    DispatchRequest(requestReceive);
                }
                catch (SerializationException serializationException)
                {
                    LogMessage($"error during deserialization : {serializationException.Message}");
                    Close();
                }
                catch (Exception e)
                {
                    Close();
                }
        }

        /// <summary>
        ///     Method to handle response that will be send to client
        /// </summary>
        private void SendingResponse()
        {
            while (State != State.DISCONNECTED)
            {
                if (ResponseQueue.Count > 0)
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

                Thread.Sleep(30);
            }
        }

        /// <summary>
        ///     Dispatch request incoming to the correct action
        /// </summary>
        /// <param name="request">The request send by the user</param>
        private void DispatchRequest(Request request)
        {
            switch (request.Type)
            {
                case Command.Login:
                    AuthManager.HandleLogin(request);
                    break;
                case Command.Register:
                    AuthManager.HandleRegister(request);
                    break;
                case Command.PrivateMessage:
                    MessageManager.HandlePrivateMessageSend(request);
                    break;
                case CreateTopic:
                    TopicManager.CreateTopic(request);
                    break;
                case ListTopics:
                    TopicManager.ListTopic();
                    break;
                case JoinTopic:
                    TopicManager.JoinTopic(request, AuthManager.CurrentUser.Username);
                    break;
                case MessageTopic:
                    TopicManager.SendMessageInTopic(request);
                    break;
                case LeaveTopic:
                    TopicManager.RemoveUserFromTopic(request);
                    break;
            }
        }

        private void LogMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0} {1}", _consoleName, message);
        }

        private void Close()
        {
            LogMessage("connection lost, removing client...");
            State = State.DISCONNECTED;
            // invoke event to remove client manager from the list 
            CloseConnectionEvent?.Invoke(this, this);
        }
    }
}