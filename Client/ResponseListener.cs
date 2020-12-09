using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Client
{
    public class ResponseListener
    {
        private readonly ClientManager _client;

        public ResponseListener(ClientManager client)
        {
            _client = client;
        }

        public event EventHandler<Response> ResponseEvent;

        public void Start(string threadName)
        {
            var responseListenerThread = new Thread(Listen);
            responseListenerThread.Name = threadName;
            responseListenerThread.Start();
        }

        private void Listen()
        {
            //Console.WriteLine("response listener is listening in thread {0}", Thread.CurrentThread.Name);
            var formatter = new BinaryFormatter();
            while (_client.State != State.DISCONNECTED)
                try
                {
                    Thread.Sleep(300);
                    var response = (Response) formatter.Deserialize(_client.Stream);
                    // send the response in action event method by invoking 
                    ResponseEvent?.Invoke(this, response);
                }
                catch (IOException e)
                {
                    // TODO remove in prod
                    Console.WriteLine(e.Message);
                    _client.Close();
                }
        }
    }
}