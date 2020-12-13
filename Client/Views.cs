using System;
using System.Collections.Generic;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Client
{
    public class Views
    {
        public static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("+--------Chat App--------+\n" +
                              "1 - Send private message\n" +
                              "2 - My private messages ({0})\n" +
                              "3 - Create a topic\n" +
                              "4 - List of topics\n" +
                              "5 - Join a topic\n" +
                              "0 - Exit\n" +
                              "+------------------------+\n",
                ClientManager.MessageManager.MyPrivateMessages.Count.ToString());
        }

        public static void DisplayPrivateMessage(List<PrivateMessage> messages)
        {
            Console.Clear();
            Console.WriteLine("+-List private messages-+");

            for (var i = 0; i < messages.Count; i++)
                Console.WriteLine("{0} - message from {1}:\n{2}", i, messages[i].SenderUsername, messages[i].Content);

            Console.WriteLine("+-----------------------+");
            Console.WriteLine("Tap any key to return to menu");
            Console.ReadKey();
        }

        public static void DisplayListTopics(Response response)
        {
            Console.Clear();
            Console.WriteLine("+------Topics list-----+");
            ((List<Topic>) response.Body).ForEach(Console.WriteLine);
            Console.WriteLine("+----------------------+");
            Console.WriteLine("Tap any key to return to menu");
            Console.ReadKey();
        }

        public static void DisplayAuthMenu()
        {
            Console.Clear();
            Console.WriteLine("+--------Chat App--------+\n" +
                              "1 - Login\n" +
                              "2 - Register\n" +
                              "0 - Exit\n" +
                              "+------------------------+\n");
        }
    }
}