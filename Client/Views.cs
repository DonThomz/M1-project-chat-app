using System;

namespace Client
{
    public class Views
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("1 - Send private message\n" +
                              "2 - My private messages ({0})\n" +
                              "3 - Exit", ClientManager.MessageManager.MyPrivateMessages.Count.ToString());
        }

        public static string DisplayPrivateMessage()
        {
            return "";
        }
    }
}