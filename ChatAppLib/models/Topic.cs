using System;
using System.Collections.Generic;

namespace ChatAppLib.models
{
    [Serializable]
    public class Topic
    {
        public Topic()
        {
        }

        public Topic(string title)
        {
            Id = Guid.NewGuid().ToString("N");
            Title = title;
            Members = new List<User>();
            Messages = new List<Message>();
        }

        public Topic(string title, List<User> members)
        {
            Title = title;
            Members = members;
        }

        public Topic(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public Topic(string id, string title, List<User> members)
        {
            Id = id;
            Title = title;
            Members = members;
            Messages = new List<Message>();
        }

        public Topic(string id, string title, List<User> members, List<Message> messages)
        {
            Id = id;
            Title = title;
            Members = members;
            Messages = messages;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public List<User> Members { get; set; }

        public List<Message> Messages { get; set; }


        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        public override string ToString()
        {
            return $"Topic : {Title}";
        }
    }
}