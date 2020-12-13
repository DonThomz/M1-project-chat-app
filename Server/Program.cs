using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ChatAppLib.models;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var server = new Server("server");
            server.Start();
        }
    }
}