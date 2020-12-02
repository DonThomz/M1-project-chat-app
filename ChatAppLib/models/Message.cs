using System;

namespace ChatAppLib
{
    public class Message
    {
        private string _id;
        private string _senderUsername;
        private string _receiverUsername;
        
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string Sender
        {
            get => _senderUsername;
            set => _senderUsername = value;
        }

        public string Receiver
        {
            get => _receiverUsername;
            set => _receiverUsername = value;
        }

        public Message(string senderUsername, string receiverUsername)
        {
            _id = Guid.NewGuid().ToString("N");
            _senderUsername = senderUsername;
            _receiverUsername = receiverUsername;
        }
    }
}