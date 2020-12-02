using System.Collections.Generic;

namespace ChatAppLib.models
{
    public class Command
    {
        private readonly string _name;

        public Command(string name)
        {
            _name = name;
        }

        public Command(string name, params KeyValuePair<string, string>[] requiredOptions)
        {
            _name = name;
            RequiredOptions = new Dictionary<string, string>();
            foreach (var requiredOption in requiredOptions)
                RequiredOptions.Add(requiredOption.Key, requiredOption.Value);
        }

        public Dictionary<string, string> RequiredOptions { get; }

        public Dictionary<string, string> Options { get; }
    }
}