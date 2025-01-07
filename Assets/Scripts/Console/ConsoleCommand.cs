using System;

namespace Console
{
    public class ConsoleCommand
    {
        public string id, desc, succ;
        event Action<string> action;

        public ConsoleCommand(string id, string desc, string succ, Action<string> action)
        {
            this.id = id;
            this.desc = desc;
            this.succ = succ;
            this.action = action;
        }

        public void Invoke(string arg)
        {
            action?.Invoke(arg);
        }
    }
}