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
        private const string PathUserFolder = "users/";
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
            Console.WriteLine(newUser);
            try
            {
                User user;
                lock (Server.Users)
                {
                    // Find user if exist
                    user = Server.Users.Find(u => u.Username.Equals(newUser.Username));
                }
                Console.WriteLine(newUser);
                if (user == null)
                {
                    Console.WriteLine(newUser.Id);
                    var fs = new FileStream(PathUserFolder + newUser.Id + ".dat", FileMode.Create);
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
                    catch (Exception e)
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
        
        public static void InitUser(List<User> users)
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) return;
            var runDir = Path.GetDirectoryName(assembly.Location) + "/" + PathUserFolder;
            var exists = System.IO.Directory.Exists(runDir);
            if (!exists) System.IO.Directory.CreateDirectory(runDir);
            var myFiles = Directory.GetFiles(runDir, "*.dat")
                .Where(file => new string[] {".dat"}
                    .Contains(Path.GetExtension(file)))
                .ToList();
            var formatter = new BinaryFormatter();
            foreach (var file in myFiles)
            {
                using var streamReader = new StreamReader(file);
                User obj;
                try
                {
                    obj = (User) formatter.Deserialize(streamReader.BaseStream);
                    Console.WriteLine(obj.Username);
                    users.Add(obj);
                }
                catch (SerializationException ex)
                {
                    throw new SerializationException(((object) ex).ToString() + "\n" + ex.Source);
                }
            }

        }
    }
}