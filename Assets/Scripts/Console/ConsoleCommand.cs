using System;

namespace Console
{
    public class ConsoleCommand
    {
        public string id, desc;
        event Action<string> action;

        public ConsoleCommand(string id, string desc, Action<string> action)
        {
            this.id = id;
            this.desc = desc;
            this.action = action;
        }

        public void Invoke(string arg)
        {
            action?.Invoke(arg);
        }
    }
}