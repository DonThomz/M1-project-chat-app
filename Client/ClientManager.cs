using System;
using System.Net;
using System.Net.Sockets;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using static ChatAppLib.models.communication.Command;

namespace Client
{
    public class ClientManager
    {
        private const int PORT = 4000;
        private const string HOSTNAME = "localhost";

        private TcpClient _client;
        private NetworkStream _stream;
        private int _portId;

        private ResponseListener _responseListener;
        private State _state;

        public TcpClient Client => _client;

        public NetworkStream Stream => _stream;

        public int PortId => _portId;

        public State State => _state;
        
        // Event 
        public event EventHandler<ClientManager> CloseConnectionEvent;
        
        // Manager 
        public static MessageManager MessageManager = new MessageManager();


        public ClientManager()
        {
            _state = State.DISCONNECTED;
        }

        public void ConnectionToServer()
        {
            while (_state == State.DISCONNECTED)
            {
                Console.WriteLine("Please, press any key to connect to the server {0}:{1}", HOSTNAME, PORT);
                Console.ReadLine();
                try
                {
                    // setup connection client-server
                    Console.WriteLine("Connection...");
                    _client = new TcpClient(HOSTNAME, PORT);
                    _portId = ((IPEndPoint) _client.Client.LocalEndPoint).Port;
                    _stream = _client.GetStream();
                    _state = State.CONNECTED;
                    Console.WriteLine("Connected !");
                    // setup connection client-server
                    
                    // launch response listener
                    LaunchResponseListener();
                    
                    // ask user to send request
                    SendRequest();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Unable to connect you to the server, please try again");
                }
            }
        }
        
        /// <summary>
        /// Display action menu and send request to the server
        /// </summary>
        private void SendRequest()
        {
            while (_state != State.DISCONNECTED)
            {
                // ask a user choice
                var inputChoice = AskForAction();
                
                // create the corresponding request
                var request = RetrieveRequest(inputChoice);
                if (request == null) break;
                try
                {
                    // send request to the server
                    var serializeRequest = Request.Serialize(request);
                    _stream.Write(serializeRequest, 0, serializeRequest.Length);
                    _stream.Flush();
                    Console.WriteLine("request send to the server");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error sending message {0} ", e.Message);
                    _state = State.DISCONNECTED;
                }
            }
        }

        /// <summary>
        /// Retrieve the request compared to the choice
        /// </summary>
        /// <param name="choice">user action choice by the user in the menu</param>
        /// <returns>The request will be send to the server</returns>
        private Request RetrieveRequest(string choice)
        {
            switch (choice)
            {
                case "1":
                    // TODO use the username 
                    return MessageManager.SendPrivateMessage(_portId.ToString());
                    break;
                case "2":
                    return null;
                    break;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Launch a thread which listen response from the server
        /// </summary>
        private void LaunchResponseListener()
        {
            // launch response listener
            _responseListener = new ResponseListener(this);
            _responseListener.ResponseEvent += RetrieveResponse;
            _responseListener.Start("my response listener");
        }

        private static string AskForAction()
        {
            // Display Menu
            Views.DisplayMenu();
            return Console.ReadLine();
        }
        
        /// <summary>
        /// Retrieve response receive from the respond listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private static void RetrieveResponse(object sender, Response response)
        {
            switch (response.Type)
            {
                case PRIVATE_MESSAGE:
                    Console.WriteLine(((PrivateMessage) response.Body).Content);
                    MessageManager.SaveMessage((PrivateMessage) response.Body);
                    break;
            }
        }
    }
}