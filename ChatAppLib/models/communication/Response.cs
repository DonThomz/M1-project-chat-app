using System;

namespace ChatAppLib
{
    public class Response
    {
        private string _id;
        private int _codeStatus;
        private string _type;
        private object _body;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public int CodeStatus
        {
            get => _codeStatus;
            set => _codeStatus = value;
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

        public Response(int codeStatus, string type)
        {
            this._id = Guid.NewGuid().ToString("N");
            this._codeStatus = codeStatus;
            this._type = type;
        }

        public Response(int codeStatus, string type, object body)
        {
            this._id = Guid.NewGuid().ToString("N");
            this._codeStatus = codeStatus;
            this._type = type;
            this._body = body;
        }

        public Response(string id, int codeStatus, string type, object body)
        {
            this._id = id;
            this._codeStatus = codeStatus;
            this._type = type;
            this._body = body;
        }
    }
}