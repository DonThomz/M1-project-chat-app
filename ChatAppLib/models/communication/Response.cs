using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ChatAppLib.models.communication
{
    [Serializable]
    public class Response
    {
        public Response(int codeStatus, string type)
        {
            Id = Guid.NewGuid().ToString("N");
            CodeStatus = codeStatus;
            Type = type;
        }

        public Response(int codeStatus, string type, object body)
        {
            Id = Guid.NewGuid().ToString("N");
            CodeStatus = codeStatus;
            Type = type;
            Body = body;
        }

        public Response(string id, int codeStatus, string type, object body)
        {
            Id = id;
            CodeStatus = codeStatus;
            Type = type;
            Body = body;
        }

        public string Id { get; set; }

        public int CodeStatus { get; set; }

        public string Type { get; set; }

        public object Body { get; set; }
        
        public static byte[] Serialize(Response request)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, request);
                return memoryStream.ToArray();
            }
        }

        public static Response Deserialize(byte[] message)
        {
            using (var memoryStream = new MemoryStream(message))
            {
                return (Response) new BinaryFormatter().Deserialize(memoryStream);
            }
        }
    }
}