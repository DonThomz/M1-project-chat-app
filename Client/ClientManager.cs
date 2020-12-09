using System;
using System.Net;
using System.Net.Sockets;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using Client.Manager;
using static ChatAppLib.models.communication.Command;
using static Client.Utils.Choice;

namespace Client
{
    public class ClientManager
    {
        private const int PORT = 4000;
        private const string HOSTNAME = "localhost";

        // Manager 
        public static MessageManager MessageManager = new MessageManager();

        private ResponseListener _responseListener;


        public ClientManager()
        {
            State = State.DISCONNECTED;
        }

        public TcpClient Client { get; private set; }

        public NetworkStream Stream { get; private set; }

        public int PortId { get; private set; }

        public State State { get; private set; }

        public void ConnectionToServer()
        {
            while (State == State.DISCONNECTED)
            {
                Console.WriteLine("Please, press any key to connect to the server {0}:{1}", HOSTNAME, PORT);
                Console.ReadLine();
                try
                {
                    // setup connection client-server
                    Console.WriteLine("Connection...");
                    Client = new TcpClient(HOSTNAME, PORT);
                    PortId = ((IPEndPoint) Client.Client.LocalEndPoint).Port;
                    Stream = Client.GetStream();
                    State = State.CONNECTED;
                    Console.WriteLine("Connected !");
                    // setup connection client-server

                    // launch response listener
                    LaunchResponseListener();

                    // ask user to send request
                    StartMenu();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Unable to connect you to the server, please try again");
                }
            }
        }

        /// <summary>
        ///     Display action menu and send request to the server
        /// </summary>
        private void StartMenu()
        {
            while (State != State.DISCONNECTED)
            {
                // ask a user choice
                var inputChoice = AskForAction();

                if (inputChoice.Equals("0")) break;
                // create the corresponding request
                RetrieveRequest(inputChoice);
                Console.ReadKey();
            }

            Console.WriteLine("Bye bye, see you later alligator !");
            Close();
        }

        /// <summary>
        ///     Retrieve the request compared to the choice
        /// </summary>
        /// <param name="choice">user action choice by the user in the menu</param>
        /// TODO use the username
        private void RetrieveRequest(string choice)
        {
            Request request = null;
            switch (choice)
            {
                case ChoicePrivateMessage:
                    request = MessageManager.SendPrivateMessage(PortId.ToString());
                    break;
                case ChoiceShowPrivateMessage:
                    Views.DisplayPrivateMessage(MessageManager.MyPrivateMessages);
                    break;
                case ChoiceCreateTopic:
                    request = TopicManager.CreateTopic(PortId.ToString());
                    break;
                case ChoiceListTopics:
                    request = TopicManager.ListTopics();
                    break;
            }

            if (request != null) SendRequest(request);
        }

        private void SendRequest(Request request)
        {
            try
            {
                // send request to the server
                var serializeRequest = Request.Serialize(request);
                Stream.Write(serializeRequest, 0, serializeRequest.Length);
                Stream.Flush();
                Console.WriteLine("\nrequest send to the server, tape to continue...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending message {0} ", e.Message);
                State = State.DISCONNECTED;
            }
        }

        /// <summary>
        ///     Launch a thread which listen response from the server
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
            Console.Write("What do you want to do ? Type a number : ");
            return Console.ReadLine();
        }

        /// <summary>
        ///     Retrieve response receive from the respond listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private static void RetrieveResponse(object sender, Response response)
        {
            switch (response.Type)
            {
                case Command.PrivateMessage:
                    MessageManager.SaveMessage((PrivateMessage) response.Body);
                    break;
                case ListTopics:
                    Views.DisplayListTopics(response);
                    break;
            }
        }

        public void Close()
        {
            Console.WriteLine("Closing connection...");
            State = State.DISCONNECTED;
            Client.Close();
        }
    }
}