using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        private const int PORT = 4000;
        private const string HOSTNAME = "localhost";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client = new TcpClient(HOSTNAME, PORT);
            var stream = client.GetStream();
            Console.ReadKey();
           
        }
    }
}