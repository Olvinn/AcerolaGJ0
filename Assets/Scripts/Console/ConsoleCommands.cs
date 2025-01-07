using System;
using System.Collections.Generic;
using System.Text;
using Controllers;
using Stages;

namespace Console
{
    public class ConsoleCommands
    {
        public Dictionary<string, ConsoleCommand> Commands = new();

        public ConsoleCommands()
        {
            List<ConsoleCommand> commands = new()
            {
                new ConsoleCommand("openStage", "opens selected stage", (x) => 
                    { StageController.Instance.OpenStage(Enum.Parse<StageType>(x)); }),
                new ConsoleCommand("backStage", "returns to previous stage", (x) =>
                    { StageController.Instance.Back(); }),
                new ConsoleCommand("hostGame", "starts multiplayer game as host", (x) =>
                    { GameController.Instance.StartServer(); }),
                new ConsoleCommand("connect", "connects to multiplayer game as client", (x) =>
                    { GameController.Instance.StartClient(); }),
                
                new ConsoleCommand("help", "shows all commands", (x) =>
                {
                    var sb = new StringBuilder();
                    foreach (var command in Commands.Values)
                        sb.AppendLine($"\"{command.id}\" : {command.desc}");
                    Console.Instance.LogText(sb.ToString()); 
                }),
            };
            
            foreach (var command in commands)
                Commands.Add(command.id, command);
        }
    }
}
