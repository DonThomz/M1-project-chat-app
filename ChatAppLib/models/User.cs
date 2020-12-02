using System;

namespace ChatAppLib
{
    public class User
    {
        private string _id;
        private string _username;
        private string _email;
        private string _password;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }
        
        

        public User(string id, string username)
        {
            _id = id;
            _username = username;
        }

        public User(string username, string email, string password)
        {
            _id = Guid.NewGuid().ToString("N");
            _username = username;
            _email = email;
            _password = password;
        }

        public User(string id, string username, string email, string password)
        {
            _id = id;
            _username = username;
            _email = email;
            _password = password;
        }
    }
}