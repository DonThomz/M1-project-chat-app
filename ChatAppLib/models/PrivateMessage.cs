using System;

namespace ChatAppLib.models
{
    [Serializable]
    public class PrivateMessage : Message
    {
        private string _receiverUsername;

        public string ReceiverUsername => _receiverUsername;

        public PrivateMessage(string senderUsername, string receiverUsername, string message) : base(senderUsername, message)
        {
            _receiverUsername = receiverUsername;
        }
        
    }
}