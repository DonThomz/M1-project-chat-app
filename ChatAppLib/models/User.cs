using System;

namespace ChatAppLib.models
{
    [Serializable]
    public class User
    {
        public User(string username)
        {
            Username = username;
        }
        
        public User(string username, string password)
        {
            Id = Guid.NewGuid().ToString("N");
            Username = username;
            Password = password;
        }
        

        public User(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public string Id { get; set; }

        public string Username { get; set; }
        
        public string Password { get; set; }
    }
}