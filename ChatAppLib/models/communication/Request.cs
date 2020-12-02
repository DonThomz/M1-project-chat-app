using System;

namespace ChatAppLib
{
    public class Request
    {
        
        private string _id;
        private string _type;
        private object _body;
        
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

        public Request(string type)
        {
            this._id = Guid.NewGuid().ToString("N");
            this._type = type;
        }
        
        public Request(string type, object body)
        {
            this._id = Guid.NewGuid().ToString("N");
            this._type = type;
            this._body = body;
        }

        public Request(string id, string type, object body)
        {
            this._id = id;
            this._type = type;
            this._body = body;
        }
    }
}