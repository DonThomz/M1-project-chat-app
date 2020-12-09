using System;

namespace ChatAppLib.models
{
    [Serializable]
    public class TopicMessage : Message
    {
        public TopicMessage(string senderUsername, string content, string topicId) : base(senderUsername, content)
        {
            TopicId = topicId;
        }

        public string TopicId { get; }
    }
}