using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Command Data", menuName = "Thimble/Commands/New Command Data")]
    public class CommandData : ScriptableObject
    {
        [Header("Dialogue Runners")]
        public List<DialogueRunner> runners = new();

        [Header("Commands")]
        public List<Command> commands = new();

        public void Register(DialogueRunner runner)
        {
            // Check if the runner is null or already registered, if it is, return
            if (runner == null || runners.Contains(runner)) return;

            // Add the runner to the set of runners
            runners.Add(runner);

            // Set up all commands for the new runner
            commands.ForEach(command => command.AddCommand(runner));
        }

        public void Unregister(DialogueRunner runner)
        {
            // Check if the runner is null or not registered, if it is, return
            if (runner == null || !runners.Contains(runner)) return;

            // Remove all commands from the runner
            commands.ForEach(command => command.RemoveCommand(runner));

            // Remove the runner from the set of runners
            runners.Remove(runner);
        }

        public bool HasRunners() => runners != null && runners.Count > 0;

        #region Command Methods

        public void AddCommand(Command command)
        {
            // Add the command to the list of commands and set the command state to inactive
            Add(command);

            // Set the command state to inactive
            command.SetCommandState(CommandState.Inactive);
        }

        public void RemoveCommand(Command command)
        {
            // Check if the command is in the list of commands, if it is, remove it
            if (commands.Contains(command))
            {
                // Deactivate the command
                DeactivateCommand(command);

                // Remove the command from the list
                Remove(command);
            }
        }

        public void RemoveCommand(string commandName)
        {
            // Find the command with the matching name, if it exists, remove it
            Command commandToRemove = commands.Find(cmd => cmd.commandName == commandName) ?? null;

            // If the command was found, remove it
            if (commandToRemove != null)
            {
                // Deactivate the command
                DeactivateCommand(commandToRemove);

                // Remove the command from the list
                Remove(commandToRemove);
            }
        }

        public void ActivateCommand(Command command)
        {
            // Add the command to the list of commands if it is not already in the list
            Add(command);

            // Add the command to each registered runner
            runners.ForEach(runner => command.AddCommand(runner));
        }

        public void DeactivateCommand(Command command)
        {
            // Check if the command is active, if not, return
            if (!command.CommandActive()) return;

            // Remove the command from each registered runner
            runners.ForEach(runner => command.RemoveCommand(runner));
        }

        [ContextMenu("Activate All Commands")]
        public void ActivateAllCommands() => commands.ForEach(command => ActivateCommand(command));

        [ContextMenu("Deactivate All Commands")]
        public void DeactivateAllCommands() => commands.ForEach(command => DeactivateCommand(command));

        [ContextMenu("Clear All Commands")]
        public void ClearAllCommands()
        {
            // Deactivate all commands before clearing the list
            DeactivateAllCommands();

            // Clear the list of commands
            Clear();
        }

        #endregion

        #region List Management

        private void Add(Command command)
        {
            // Add the command to the list if it is not already in the list
            if (!commands.Contains(command)) commands.Add(command);
        }

        private void Remove(Command command)
        {
            // Remove the command from the list if it exists
            if (commands.Contains(command)) commands.Remove(command);
        }

        private void Clear() => commands.Clear();

        #endregion
    }
}