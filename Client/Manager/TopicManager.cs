using System;
using ChatAppLib.models;
using ChatAppLib.models.communication;

namespace Client.Manager
{
    public class TopicManager
    {
        /// <summary>
        /// Display the instruction depending on step
        /// </summary>
        /// <param name="step"></param>
        /// 
        private static void DisplayInstructionToCreateTopic(int step)
        {
            switch (step)
            {
                case 0:
                    Console.Write("Please give a name : ");
                    break;
                case 1:
                    Console.Write("Do you want to join directly ? (Y/n) :");
                    break;
            }
        }

        public static Request CreateTopic(string userId)
        {
            DisplayInstructionToCreateTopic(0);
            var topicName = Console.ReadLine();
            DisplayInstructionToCreateTopic(1);
            var joinDirectly = Console.ReadKey().KeyChar == 'Y';
            var newTopic = new Topic(topicName);
            if (joinDirectly) newTopic.Members.Add(new User(userId, "user1"));
            return new Request(Command.CreateTopic, newTopic);
        }

        public static Request ListTopics()
        {
            return new Request(Command.ListTopics);
        }
    }
}