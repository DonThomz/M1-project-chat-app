using System;

namespace ChatAppLib.models
{
    public class Message
    {
        public Message(string senderUsername, string receiverUsername)
        {
            Id = Guid.NewGuid().ToString("N");
            Sender = senderUsername;
            Receiver = receiverUsername;
        }

        public string Id { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }
    }
}