using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Server.Manager
{
    public class AuthManager
    {
        public User CurrentUser { get; set; }

        // event to send a response to the user
        public event EventHandler<Response> SendResponseAuthEvent;

        public void HandleLogin(Request request)
        {
            var user = (User) request.Body;
            try
            {
                User userConnected;
                lock (Server.Users)
                {
                    userConnected = Server.Users.Find(u =>
                        u.Username.Equals(user.Username) && u.Password.Equals(user.Password));
                }

                if (userConnected == null)
                {
                    SendResponseAuthEvent?.Invoke(this, new Response(404, request.Type,
                        "wrong credentials"));
                }
                else
                {
                    // Add user
                    CurrentUser = userConnected;
                    SendResponseAuthEvent?.Invoke(this, new Response(200, request.Type,
                        userConnected));
                }
            }
            catch (ArgumentNullException)
            {
                SendResponseAuthEvent?.Invoke(this, new Response(404, request.Type,
                    "wrong credentials"));
            }
        }

        public void HandleRegister(Request request)
        {
            var newUser = (User) request.Body;
            try
            {
                User user;
                lock (Server.Users)
                {
                    // Find user if exist
                    user = Server.Users.Find(u => u.Username.Equals(newUser.Username));
                }

                if (user == null)
                {
                    var fs = new FileStream(newUser.Id + ".dat", FileMode.Create);
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        // Save user in file
                        formatter.Serialize(fs, newUser);

                        // Add the new user to the server list
                        lock (Server.Users)
                        {
                            Server.Users.Add(newUser);
                        }

                        Console.WriteLine("user added !");

                        // Add user
                        CurrentUser = newUser;
                        SendResponseAuthEvent?.Invoke(this, new Response(200, request.Type,
                            newUser));
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                        SendResponseAuthEvent?.Invoke(this, new Response(400, request.Type,
                            "error server, please try again !"));
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
                else
                    SendResponseAuthEvent?.Invoke(this, new Response(400, request.Type,
                        "already register !"));
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("error");
                SendResponseAuthEvent?.Invoke(this, new Response(400, request.Type,
                    "error server, please try again !"));
            }
        }
    }
}