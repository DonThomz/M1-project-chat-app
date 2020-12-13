using System;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Client.Manager
{
    public class AuthManager
    {
        
        public Request Login()
        {
            Console.Write("Username : ");
            var username = Console.ReadLine();
            Console.Write("Password : ");
            var password = Console.ReadLine();
            var user = new User(username, password);
            return new Request(Command.Login, user);
        }
        
        public Request Register()
        {
            Console.Write("Provide a username : ");
            var username = Console.ReadLine();
            Console.Write("Provide a password : ");
            var password = Console.ReadLine();
            var user = new User(username, password);
            return new Request(Command.Register, user);
        }
    }
}