using System;
using Yarn.Unity;

namespace Thimble
{
    public static class CommandHandler
    {
        #region Method Command Creators

        public static Command CreateCommand(string commandOrigin, string commandName, Action commandMethod, string commandDescription, string commandSyntax)
        {
            Tier0Command commandT0 = new Tier0Command(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT0;
            return command;
        }

        public static Command CreateCommand<T1>(string commandOrigin, string commandName, Action<T1> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier1Command<T1> commandT1 = new Tier1Command<T1>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT1;
            return command;
        }

        public static Command CreateCommand<T1, T2>(string commandOrigin, string commandName, Action<T1, T2> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier2Command<T1, T2> commandT2 = new Tier2Command<T1, T2>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT2;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3>(string commandOrigin, string commandName, Action<T1, T2, T3> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier3Command<T1, T2, T3> commandT3 = new Tier3Command<T1, T2, T3>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT3;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4>(string commandOrigin, string commandName, Action<T1, T2, T3, T4> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier4Command<T1, T2, T3, T4> commandT4 = new Tier4Command<T1, T2, T3, T4>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT4;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5>(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier5Command<T1, T2, T3, T4, T5> commandT5 = new Tier5Command<T1, T2, T3, T4, T5>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT5;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6>(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier6Command<T1, T2, T3, T4, T5, T6> commandT6 = new Tier6Command<T1, T2, T3, T4, T5, T6>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT6;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7>(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier7Command<T1, T2, T3, T4, T5, T6, T7> commandT7 = new Tier7Command<T1, T2, T3, T4, T5, T6, T7>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT7;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7, T8>(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7, T8> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier8Command<T1, T2, T3, T4, T5, T6, T7, T8> commandT8 = new Tier8Command<T1, T2, T3, T4, T5, T6, T7, T8>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT8;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier9Command<T1, T2, T3, T4, T5, T6, T7, T8, T9> commandT9 = new Tier9Command<T1, T2, T3, T4, T5, T6, T7, T8, T9>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT9;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> commandMethod, string commandDescription, string commandSyntax)
        {
            Tier10Command<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> commandT10 = new Tier10Command<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(commandOrigin, commandName, commandMethod, commandDescription, commandSyntax);
            Command command = commandT10;
            return command;
        }

        #endregion

        #region Tier Command Creators

        public static Command CreateCommand(Tier0Command commandT0)
        {
            Command command = commandT0;
            return command;
        }

        public static Command CreateCommand<T1>(Tier1Command<T1> commandT1)
        {
            Command command = commandT1;
            return command;
        }

        public static Command CreateCommand<T1, T2>(Tier2Command<T1, T2> commandT2)
        {
            Command command = commandT2;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3>(Tier3Command<T1, T2, T3> commandT3)
        {
            Command command = commandT3;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4>(Tier4Command<T1, T2, T3, T4> commandT4)
        {
            Command command = commandT4;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5>(Tier5Command<T1, T2, T3, T4, T5> commandT5)
        {
            Command command = commandT5;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6>(Tier6Command<T1, T2, T3, T4, T5, T6> commandT6)
        {
            Command command = commandT6;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7>(Tier7Command<T1, T2, T3, T4, T5, T6, T7> commandT7)
        {
            Command command = commandT7;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7, T8>(Tier8Command<T1, T2, T3, T4, T5, T6, T7, T8> commandT8)
        {
            Command command = commandT8;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Tier9Command<T1, T2, T3, T4, T5, T6, T7, T8, T9> commandT9)
        {
            Command command = commandT9;
            return command;
        }

        public static Command CreateCommand<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Tier10Command<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> commandT10)
        {
            Command command = commandT10;
            return command;
        }

        #endregion

        #region Command Data Methods

        public static void AddCommand(CommandData commandData, Command command, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            commandData.runner = runner;
            commandData.AddCommand(command);
        }

        public static void RemoveCommand(CommandData commandData, Command command, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            commandData.runner = runner;
            commandData.RemoveCommand(command);
        }

        public static void RemoveCommand(CommandData commandData, string commandName, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            commandData.runner = runner;
            commandData.RemoveCommand(commandName);
        }

        public static void ActivateCommand(CommandData commandData, Command command, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            commandData.runner = runner;
            commandData.ActivateCommand(command);
        }

        public static void DeactivateCommand(CommandData commandData, Command command, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            commandData.runner = runner;
            commandData.DeactivateCommand(command);
        }

        #endregion
    }
}