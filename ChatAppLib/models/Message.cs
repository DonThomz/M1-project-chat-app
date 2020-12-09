using System;

namespace ChatAppLib.models
{
    [Serializable]
    public abstract class Message
    {
        private string _content;

        public Message(string senderUsername, string content)
        {
            Id = Guid.NewGuid().ToString("N");
            SenderUsername = senderUsername;
            _content = content;
        }

        public string Id { get; }

        public string SenderUsername { get; }

        public string Content
        {
            get => _content;
            set => _content = value;
        }
    }
}