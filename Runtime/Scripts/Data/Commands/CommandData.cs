using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Command Data", menuName = "Thimble/Commands/New Command Data")]
    public class CommandData : ScriptableObject
    {
        [Header("Dialogue Runner")]
        public DialogueRunner runner;

        [Header("Commands")]
        public List<Command> commands = new();

        public void SetRunner(DialogueRunner runner) => this.runner = runner;

        #region Command Methods

        public void AddCommand(Command command)
        {
            // Add the command to the list of commands and set the command state to inactive
            Add(command);
            command.SetCommandState(CommandState.Inactive);
        }

        public void RemoveCommand(Command command)
        {
            // Check if the command is in the list of commands, if it is, remove it
            if (commands.Contains(command))
            {
                DeactivateCommand(command);
                Remove(command);
            }
        }

        public void RemoveCommand(string commandName)
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

            // If the command was found, deactivate it and remove it from the list
            if (commandToRemove != null)
            {
                DeactivateCommand(commandToRemove);
                Remove(commandToRemove);
            }
        }

        public void ActivateCommand(Command command)
        {
            // Add the command to the list of commands if it is not already in the list
            Add(command);

            // Add the command to the dialogue runner
            command.AddCommand(runner);
        }

        public void DeactivateCommand(Command command)
        {
            if (!command.CommandActive()) return;
            else command.RemoveCommand(runner);
        }

        public void DeactivateAllCommands() => commands.ForEach(command => DeactivateCommand(command));

        public void ActivateAllCommands() => commands.ForEach(command => ActivateCommand(command));

        public void ClearAllCommands()
        {
            DeactivateAllCommands();
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

        public void Clear() => commands.Clear();

        #endregion
    }
}