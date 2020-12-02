using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ChatAppLib.models.communication
{
    [Serializable]
    public class Request
    {
        private object _body;
        private string _id;
        private string _type;

        public Request(string type)
        {
            _id = Guid.NewGuid().ToString("N");
            _type = type;
        }

        public Request(string type, object body)
        {
            _id = Guid.NewGuid().ToString("N");
            _type = type;
            _body = body;
        }

        public Request(string id, string type, object body)
        {
            _id = id;
            _type = type;
            _body = body;
        }

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string Type
        {
            get => _type;
            set => _type = value;
        }

        public object Body
        {
            get => _body;
            set => _body = value;
        }

        public static byte[] Serialize(Request request)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, request);
                return memoryStream.ToArray();
            }
        }

        public static Request Deserialize(byte[] message)
        {
            using (var memoryStream = new MemoryStream(message))
            {
                return (Request) new BinaryFormatter().Deserialize(memoryStream);
            }
        }

        public override string ToString()
        {
            return $"Request n°{_id}: \n" +
                   $"type: {_type}\n" +
                   $"body: {_body}";
        }
    }
}