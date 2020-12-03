using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Client
{
    public class ResponseListener
    {
        private ClientManager _client;
        
        public event EventHandler<Response> ResponseEvent;

        public ResponseListener(ClientManager client)
        {
            _client = client;
        }

        public void Start(string threadName)
        {
            var responseListenerThread = new Thread(Listen);
            responseListenerThread.Name = threadName;
            responseListenerThread.Start();
        }

        private void Listen()
        {
            Console.WriteLine("response listener is listening in thread {0}", Thread.CurrentThread.Name);
            while (_client.State == State.CONNECTED)
            {
                var formatter = new BinaryFormatter();
                var response = (Response) formatter.Deserialize(_client.Stream);
                // send the response in action event method by invoking 
                ResponseEvent?.Invoke(this, response);
            }
        }
    }
}