using System;
using System.Collections.Generic;

namespace ChatAppLib
{
    public class Topic
    {
        private string _id;
        private string _title;
        private List<User> _members;
        private List<Message> _messages;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public List<User> Members
        {
            get => _members;
            set => _members = value;
        }

        public List<Message> Messages
        {
            get => _messages;
            set => _messages = value;
        }


        public void AddMessage(Message message)
        {
            this._messages.Add(message);
        }
        
        public Topic(string title)
        {
            this._id = Guid.NewGuid().ToString("N");
            this._title = title;
            this._members = new List<User>();
            this._messages = new List<Message>();
        }

        public Topic(string title, List<User> members)
        {
            _title = title;
            _members = members;
        }

        public Topic(string id, string title)
        {
            this._id = id;
            this._title = title;
        }

        public Topic(string id, string title, List<User> members)
        {
            this._id = id;
            this._title = title;
            this._members = members;
            this._messages = new List<Message>();
        }
        
        public Topic(string id, string title, List<User> members, List<Message> messages)
        {
            this._id = id;
            this._title = title;
            this._members = members;
            this._messages = messages;
        }
    }
}