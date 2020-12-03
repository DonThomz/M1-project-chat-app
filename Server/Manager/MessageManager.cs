using System;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using static ChatAppLib.models.communication.Command;
namespace Server.Manager
{
    public class MessageManager
    {
        public event EventHandler<Response> SendResponseMessageEvent;

        public MessageManager()
        {
        }
        
        /// <summary>
        /// Create a response back to send a private message to an user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response SendPrivateMessageBack(Request request)
        {
            // create a private message
            var messageObject = (PrivateMessage) request.Body;

            // return the request
            return new Response(200,PRIVATE_MESSAGE, messageObject);
        }
        
        /// <summary>
        /// Create a response back to send a private message to an user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response SendPrivateMessage(Request request)
        {
            // create a private message
            var messageObject = (PrivateMessage) request.Body;

            // return the request
            return new Response(200,PRIVATE_MESSAGE, messageObject);
        }

        public void HandlePrivateMessageSend(Request request)
        {
            var privateMessage = (PrivateMessage) request.Body;
            // TODO send call back response to sender
            
            
            // TODO send private message to receiver 
            try
            {
                Receiver receiver = Server.Clients.Find(c => c.RemotePort.ToString().Equals(privateMessage.ReceiverUsername));
                // Invoke SendResponseMessage Event
                receiver?.MessageManager.SendResponseMessageEvent?.Invoke(this, new Response(200, request.Type, request.Body));
                Console.WriteLine("Message send to user {0}",privateMessage.ReceiverUsername);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("receiver {0} not found", privateMessage.ReceiverUsername);
            }
        }
    }
}