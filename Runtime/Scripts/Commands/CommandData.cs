using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Command Data", menuName = "Thimble/Commands/New Command Data")]
    public class CommandData : ScriptableObject
    {
        [Header("Dialogue Runner")]
        public DialogueRunner dialogueRunner;

        [Header("Commands")]
        public List<Command> commands = new List<Command>();

        #region Command Methods

        public void AddCommand(DialogueRunner runner, Command command)
        {
            // Add the command to the list of commands and set the command state to inactive
            Add(command);
            command.SetCommandState(CommandState.Inactive);
        }

        public void RemoveCommand(DialogueRunner runner, Command command)
        {
            // Check if the command is in the list of commands, if it is, remove it
            if (commands.Contains(command))
            {
                DeactivateCommand(runner, command);
                Remove(command);
            }
        }

        public void RemoveCommand(DialogueRunner runner, string commandName)
        {
            // Find the command with the matching name, if it exists, remove it
            Command commandToRemove = null;
            foreach (Command command in commands)
            {
                if (command.commandName == commandName)
                {
                    commandToRemove = command;
                    break;
                }
            }

            if (commandToRemove != null)
            {
                DeactivateCommand(runner, commandToRemove);
                Remove(commandToRemove);
            }
        }

        public void ActivateAllCommands(DialogueRunner runner)
        {
            foreach (Command command in commands)
            {
                ActivateCommand(runner, command);
            }
        }

        public void ActivateCommand(DialogueRunner runner, Command command)
        {
            // Add the command to the list of commands if it is not already in the list
            Add(command);

            // Add the command to the dialogue runner
            command.AddCommand(runner);
        }

        public void DeactivateAllCommands(DialogueRunner runner)
        {
            foreach (Command command in commands)
            {
                DeactivateCommand(runner, command);
            }
        }

        public void DeactivateCommand(DialogueRunner runner, Command command)
        {
            if (!command.CommandActive()) return;
            else command.RemoveCommand(runner);
        }

        public void ClearAllCommands(DialogueRunner runner)
        {
            DeactivateAllCommands(runner);
            commands.Clear();
        }

        #endregion

        #region List Management

        public void Add(Command command)
        {
            if (!commands.Contains(command)) commands.Add(command);
        }

        public void Remove(Command command)
        {
            if (commands.Contains(command)) commands.Remove(command);
        }

        #endregion
    }
}