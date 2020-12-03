﻿using System;
using System.Collections.Generic;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using static ChatAppLib.models.communication.Command;
namespace Client
{
    /// <summary>
    /// <para/>Use to :
    /// <para/>- send private message
    /// <para/>- stock private messages
    /// <para/>- retrieve private message
    /// <para/>- send message in a topic
    /// 
    /// </summary>
    public class MessageManager
    {

        private List<PrivateMessage> _myPrivateMessages;

        public List<PrivateMessage> MyPrivateMessages => _myPrivateMessages;

        public void SaveMessage(PrivateMessage message)
        {
            _myPrivateMessages.Add(message);
        }
        
        public MessageManager()
        {
            // TODO use serialization to retrieve private messages 
            _myPrivateMessages = new List<PrivateMessage>();
        }
        
        /// <summary>
        /// Create a request to send a private message to an user
        /// </summary>
        /// <param name="senderUsername">The message sender</param>
        /// <returns>A request containing the message that will be send</returns>
        public Request SendPrivateMessage(string senderUsername)
        {
            DisplayInstruction(0);
            var receiverUsername = Console.ReadLine();
            DisplayInstruction(1);
            var message = Console.ReadLine();

            // create a private message
            var messageObject = new PrivateMessage(senderUsername, receiverUsername, message);

            // return the request
            return new Request(PRIVATE_MESSAGE, messageObject);
        }

        /// <summary>
        /// Display the instruction depending on step
        /// </summary>
        /// <param name="step"></param>
        /// 
        private void DisplayInstruction(int step)
        {
            switch (step)
            {
                case 0:
                    Console.WriteLine("which user do you want to send a private message ? [username]");
                    break;
                case 1:
                    Console.WriteLine("what is your message ?");
                    break;
            }
        }
    }
}