using System;

namespace ChatAppLib.models
{
    [Serializable]
    public class PrivateMessage : Message
    {
        public PrivateMessage(string senderUsername, string receiverUsername, string message) : base(senderUsername,
            message)
        {
            ReceiverUsername = receiverUsername;
        }

        public string ReceiverUsername { get; }
    }
}