namespace ChatAppLib.models.communication
{
    public static class Command
    {
        public const string PrivateMessage = "mp";
        public const string PrivateMessageConfirm = "mp";
        public const string MessageTopic = "mt";
        public const string JoinTopic = "join-topic";
        public const string CreateTopic = "create-topic";
        public const string ListTopics = "list-topics";
        public const string Login = "login";
        public const string Register = "register";
    }
}