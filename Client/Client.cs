using System;
using System.Net.Sockets;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Client
{
    public class Client
    {
        private const int PORT = 4000;
        private const string HOSTNAME = "localhost";
        private TcpClient _client;

        private ResponseListener _responseListener;
        private State _state;
        private NetworkStream _stream;

        public Client()
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
                    Console.WriteLine("Connection...");
                    _client = new TcpClient(HOSTNAME, PORT);
                    _stream = _client.GetStream();
                    _state = State.CONNECTED;
                    // launch response listener
                    _responseListener = new ResponseListener("my response listener");

                    Console.WriteLine("Connected !");
                    SendRequest();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Unable to connect you to the server, please try again");
                }
            }
        }

        private void SendRequest()
        {
            while (_state != State.DISCONNECTED)
            {
                var inputReadLine = Console.ReadLine();
                // send a request
                var request = new Request(inputReadLine);
                try
                {
                    var serializeRequest = Request.Serialize(request);
                    Console.WriteLine(serializeRequest.Length);
                    _stream.Write(serializeRequest, 0, serializeRequest.Length);
                    _stream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error sending message {0} ", e.Message);
                }
            }
        }
    }
}