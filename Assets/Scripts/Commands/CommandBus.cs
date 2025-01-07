using System;
using System.Collections.Generic;

namespace Commands
{
    public delegate void CommandHandler<T>(T command) where T : struct;

    public class CommandBus
    {
        private static CommandBus _instance;

        public static CommandBus Instance
        {
            get
            {
                return _instance ??= new CommandBus();
            }
        }

        private Dictionary<Type, Delegate> _handlers;

        public CommandBus()
        {
            _handlers = new Dictionary<Type, Delegate>();
        }

        public void Reset()
        {
            _handlers = new Dictionary<Type, Delegate>();
        }

        public void RegisterHandler<T>(CommandHandler<T> command) where T : struct
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type)) _handlers.Add(type, command);
            else _handlers[type] = Delegate.Combine(_handlers[type], command);
        }

        public void RemoveHandler<T>(CommandHandler<T> command) where T : struct
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
                _handlers[type] = Delegate.Remove(_handlers[type], command);
            _handlers.Remove(type);
        }

        public void Handle<T>(T command) where T : struct
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type)) return;
            ((CommandHandler<T>)_handlers[type]).Invoke(command);
        }
    }
}