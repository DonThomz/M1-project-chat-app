using System;
using System.Collections.Generic;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using static ChatAppLib.models.communication.Command;

namespace Server.Manager
{
    public class TopicManager
    {
        public static List<Topic> topics = new List<Topic>();
        public event EventHandler<Response> SendResponseTopicEvent;

        public void CreateTopic(Request request)
        {
            var topic = (Topic) request.Body;
            lock (topics)
            {
                topics.Add(topic);
            }
        }

        public void ListTopic(Request request)
        {
            var response = new Response(200, ListTopics, topics);
            SendResponseTopicEvent?.Invoke(this, response);
        }
    }
}