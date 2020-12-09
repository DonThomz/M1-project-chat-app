using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using static ChatAppLib.models.communication.Command;

namespace Server.Manager
{
    public class TopicManager
    {
        // list of topics available
        private static readonly List<Topic> Topics = new List<Topic>();

        private static readonly ConcurrentDictionary<string, Topic> TopicsMap =
            new ConcurrentDictionary<string, Topic>();

        // event to send a response to the user
        public event EventHandler<Response> SendResponseTopicEvent;

        /// <summary>
        ///     Handle the request to create a new topic
        /// </summary>
        /// <param name="request"></param>
        public void CreateTopic(Request request)
        {
            var topic = (Topic) request.Body;
            if (TopicsMap.ContainsKey(topic.Title)) Console.WriteLine("Topic {0} already exist", topic.Title);

            TopicsMap.TryAdd(topic.Title, topic);
            var response = new Response(200, Command.CreateTopic, topic);
            SendResponseTopicEvent?.Invoke(this, response);
            /*lock (Topics)
            {
                Topics.Add(topic);
                // Send response with the topic
                var response = new Response(200, Command.CreateTopic, topic);
                SendResponseTopicEvent?.Invoke(this, response);
            }*/
        }

        /// <summary>
        ///     Handle the request to get the list of topic
        /// </summary>
        public void ListTopic()
        {
            // send response with the list of topics
            var response = new Response(200, ListTopics, TopicsMap.Values.ToList());
            SendResponseTopicEvent?.Invoke(this, response);
        }

        /// <summary>
        ///     Handle the request to join a topic
        /// </summary>
        /// <param name="request"></param>
        public void JoinTopic(Request request, string userId)
        {
            var topicName = (string) request.Body;
            if (!TopicsMap.TryGetValue(topicName, out var topic))
            {
                // send a 404 error response with the topic name
                var response = new Response(404, Command.JoinTopic, $"Topic {topicName} not found");
                SendResponseTopicEvent?.Invoke(this, response);
            }
            else
            {
                topic?.Members.Add(new User(userId, userId));
                // send the response with the topic
                var response = new Response(200, Command.JoinTopic, topic);
                SendResponseTopicEvent?.Invoke(this, response);
            }
        }

        /// <summary>
        ///     Send a message to all chatter in a room
        /// </summary>
        /// <param name="request"></param>
        public void SendMessageInTopic(Request request)
        {
            var topicMessage = (TopicMessage) request.Body;
            TopicsMap.TryGetValue(topicMessage.TopicId, out var topic);
            if (topic == null) return;
            lock (topic.Messages)
            {
                if (!topicMessage.SenderUsername.Equals("server")) topic.Messages.Add(topicMessage);
                Console.WriteLine("Message add !");
                Console.WriteLine("size : " + Server.Clients
                    .FindAll(r => topic.Members.Exists(u => u.Username == r.RemotePort.ToString())).Count);
                // send responses back 
                Server.Clients.FindAll(r => topic.Members.Exists(u => u.Username == r.RemotePort.ToString()))
                    .ForEach(r =>
                    {
                        var response = new Response(200, MessageTopic, topicMessage);
                        r.TopicManager.SendResponseTopicEvent(this, response);
                    });
            }
        }
    }
}