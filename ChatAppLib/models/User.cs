using System;

namespace ChatAppLib.models
{
    [Serializable]
    public class User
    {
        public User(string id, string username)
        {
            Id = id;
            Username = username;
        }

        public User(string username, string email, string password)
        {
            Id = Guid.NewGuid().ToString("N");
            Username = username;
            Email = email;
            Password = password;
        }

        public User(string id, string username, string email, string password)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}