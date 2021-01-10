using System;
using System.Collections.Generic;
using System.IO;
using ChatAppLib.models;
using ChatAppLib.models.communication;
using static ChatAppLib.models.communication.Command;

namespace Client.Manager
{
    public class TopicManager
    {
        public Queue<TopicMessage> MessagesQueue;

        public Topic CurrentTopic { get; private set; }

        public event EventHandler<State> ChangeStateEvent;
        public event EventHandler<Request> SendTopicMessageEvent;

        /// <summary>
        /// Display the instruction depending on step
        /// </summary>
        /// <param name="step"></param>
        /// 
        private void DisplayInstructionToCreateTopic(int step)
        {
            switch (step)
            {
                case 0:
                    Console.Write("Please give a name : ");
                    break;
            }
        }

        public Request CreateTopic(string userId)
        {
            DisplayInstructionToCreateTopic(0);
            var topicName = Console.ReadLine();
            DisplayInstructionToCreateTopic(1);
            var newTopic = new Topic(topicName);
            return new Request(Command.CreateTopic, newTopic);
        }

        public Request ListTopics()
        {
            return new Request(Command.ListTopics);
        }

        public Request AskForJoinTopic()
        {
            Console.Write("Which topics ? ");
            var topicName = Console.ReadLine();
            return new Request(Command.JoinTopic, topicName);
        }

        protected virtual void OnChangeStateEvent(State e)
        {
            ChangeStateEvent?.Invoke(this, e);
        }

        public void JoinTopic(Response response, string userId)
        {
            CurrentTopic = (Topic) response.Body;
            Console.WriteLine(CurrentTopic);
            if (CurrentTopic == null)
            {
                Console.WriteLine("Sorry topic not found :(");
            }
            else
            {
                MessagesQueue = new Queue<TopicMessage>();
                // Send message from server to indicate the user who joining the room
                OnChangeStateEvent(State.IN_TOPIC);
            }
        }

        public void LaunchTopicRoom(string userId)
        {
            
            Request request;
            string message;
            var exist = false;
            var error = false;
            Console.Clear();
            Console.WriteLine("+----{0}----+", CurrentTopic.Title);
            Console.WriteLine("Currently users in topic : {0}", CurrentTopic.Members.Count.ToString());
            Console.WriteLine("Send message or tape 'exit' to quit ");
            CurrentTopic.Messages.ForEach(m => Console.WriteLine("{0} > {1}", m.SenderUsername, m.Content));
            var messageComingInRoom = new TopicMessage("server", $"{userId} join the room", CurrentTopic.Title); 
            request = new Request(MessageTopic, messageComingInRoom);
            SendTopicMessageEvent?.Invoke(this, request);
            do
            {
                try
                {
                    message = Console.ReadLine();
                    if (message != null)
                    {
                        if (message.Equals("exit"))
                        {
                            exist = true;
                            Console.WriteLine("tape a key to return the menu...");
                        }
                        else
                        {
                            var messageTopic = new TopicMessage(userId, message, CurrentTopic.Title); 
                            request = new Request(MessageTopic, messageTopic);
                            SendTopicMessageEvent?.Invoke(this, request);
                            //Console.WriteLine("{0} > {1}", messagTopic.SenderUsername, messagTopic.Content);
                            
                        }
                    }
                    else
                    {
                        error = true;
                    }
                }
                catch (Exception e) when (
                    e is IOException
                    || e is OutOfMemoryException
                    || e is ArgumentOutOfRangeException
                )
                {
                    Console.WriteLine(e.Message);
                    error = true;
                }
            } while (!error && !exist);
            SendTopicMessageEvent?.Invoke(this, new Request(LeaveTopic, new TopicMessage(userId, "", CurrentTopic.Title)));
            OnChangeStateEvent(State.CONNECTED);
        }

        public void AddMessageReceive(Response response, string userId)
        {
            var message = (TopicMessage) response.Body;
            
            if (message.SenderUsername.Equals("server"))
            {
                
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("{0} > {1}", message.SenderUsername, message.Content);
                Console.ResetColor();
            }
            else if (!message.SenderUsername.Equals(userId) && !userId.Equals("server"))
            {
                CurrentTopic.Messages.Add(message);
                Console.WriteLine("{0} > {1}", message.SenderUsername, message.Content);
            }
            else if (message.SenderUsername.Equals(userId))
            {
                CurrentTopic.Messages.Add(message);
            }
        }
    }
}