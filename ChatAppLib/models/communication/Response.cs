using System;

namespace ChatAppLib.models.communication
{
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
    }
}