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
            Members = new HashSet<string>();
            Messages = new List<Message>();
        }

        public Topic(string title, HashSet<string> members)
        {
            Title = title;
            Members = members;
        }

        public Topic(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public Topic(string id, string title, HashSet<string> members)
        {
            Id = id;
            Title = title;
            Members = members;
            Messages = new List<Message>();
        }

        public Topic(string id, string title, HashSet<string> members, List<Message> messages)
        {
            Id = id;
            Title = title;
            Members = members;
            Messages = messages;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public HashSet<string> Members { get; set; }

        public List<Message> Messages { get; set; }


        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        public void AddUser(string userId)
        {
            Members.Add(userId);
        }
        
        public void RemoveUser(string userId)
        {
            Members.RemoveWhere(u => u.Equals(userId));
        }


        public override string ToString()
        {
            return $"Topic : {Title}";
        }
    }
}