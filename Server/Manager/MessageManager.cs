using System;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Server.Manager
{
    public class MessageManager
    {
        // event to send a response to the user
        public event EventHandler<Response> SendResponseMessageEvent;

        /// <summary>
        ///     Handle a private message incoming and send back response to sender and receiver
        /// </summary>
        /// <param name="request">The sender user request</param>
        public void HandlePrivateMessageSend(Request request)
        {
            var privateMessage = (PrivateMessage) request.Body;

            // TODO send private message to receiver 
            try
            {
                // Find the receiver in the list of client available
                var receiver = Server.Clients.Find(c => c.AuthManager.CurrentUser.Username.Equals(privateMessage.ReceiverUsername));
                if (receiver == null)
                {
                    SendResponseMessageEvent?.Invoke(this, new Response(404, request.Type, "user not found"));
                    return;
                }
                
                // Send response to receiver if exit
                receiver?.MessageManager.SendResponseMessageEvent?.Invoke(this,
                    new Response(200, request.Type, request.Body));

                // Send response to sender
                SendResponseMessageEvent?.Invoke(this, new Response(200, request.Type, request.Body));

                Console.WriteLine("Message send to user {0}", privateMessage.ReceiverUsername);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("receiver {0} not found", privateMessage.ReceiverUsername);
            }
        }
    }
}