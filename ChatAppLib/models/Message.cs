using System;

namespace ChatAppLib.models
{
    [Serializable]
    public abstract class Message
    {
        private string _id;

        private string _senderUsername;
        private string _content;

        public string Id => _id;
        public string SenderUsername => _senderUsername;

        public string Content
        {
            get => _content;
            set => _content = value;
        }

        public Message(string senderUsername, string content)
        {
            _id = Guid.NewGuid().ToString("N");
            _senderUsername = senderUsername;
            _content = content;
        }
        
    }
}