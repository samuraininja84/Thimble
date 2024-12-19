using System;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace Thimble
{
    [Serializable]
    public class Command : IRegisterCommands
    {
        public CommandState commandState = CommandState.Inactive;
        [HideInInspector] public CommandTier commandTier = CommandTier.Tier0;
        public string commandOrigin;
        public string commandName;
        public string commandDescription;
        public string commandSyntax;

        #region Virtual Methods

        public virtual void AddCommand(DialogueRunner dialogueRunner) { }

        public virtual void RemoveCommand(DialogueRunner dialogueRunner) { }

        #endregion

        #region Public Methods

        public void SetCommandState(CommandState state)
        {
            commandState = state;
        }

        public string GetCommandInfo()
        {
            return $"Command Tier: {CommandTierToString()}\nCommand Origin: {commandOrigin}\nCommand Name: {commandName}\nCommand Description: {commandDescription}\nCommand Syntax: {commandSyntax}";
        }

        public string CommandTierToString()
        {
            return commandTier.ToString().Insert(4, " ");
        }

        public bool CommandActive()
        {
            return commandState == CommandState.Active;
        }

        #endregion
    }

    public enum CommandState
    {
        Inactive,
        Active
    }

    public enum CommandTier
    {
        Tier0,
        Tier1,
        Tier2,
        Tier3,
        Tier4,
        Tier5,
        Tier6,
        Tier7,
        Tier8,
        Tier9,
        Tier10
    }

    #region Command Tiers

    [Serializable]
    public class Tier0Command : Command
    {
        public Action commandMethod;
        public UnityEvent commandEvent;

        public Tier0Command(string commandOrigin, string commandName, Action commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier0;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            commandMethod += commandEvent.Invoke;
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandMethod -= commandEvent.Invoke;
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier1Command<T1> : Command
    {
        public Action<T1> commandMethod;
        public UnityEvent<T1> commandEvent;

        public Tier1Command(string commandOrigin, string commandName, Action<T1> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier1;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            commandMethod += commandEvent.Invoke;
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandMethod -= commandEvent.Invoke;
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier2Command<T1, T2> : Command
    {
        public Action<T1, T2> commandMethod;
        public UnityEvent<T1, T2> commandEvent;

        public Tier2Command(string commandOrigin, string commandName, Action<T1, T2> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier2;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            commandMethod += commandEvent.Invoke;
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandMethod -= commandEvent.Invoke;
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier3Command<T1, T2, T3> : Command
    {
        public Action<T1, T2, T3> commandMethod;
        public UnityEvent<T1, T2, T3> commandEvent;

        public Tier3Command(string commandOrigin, string commandName, Action<T1, T2, T3> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier3;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            commandMethod += commandEvent.Invoke;
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandMethod -= commandEvent.Invoke;
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier4Command<T1, T2, T3, T4> : Command
    {
        public Action<T1, T2, T3, T4> commandMethod;
        public UnityEvent<T1, T2, T3, T4> commandEvent;

        public Tier4Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier4;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            commandMethod += commandEvent.Invoke;
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandMethod -= commandEvent.Invoke;
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier5Command<T1, T2, T3, T4, T5> : Command
    {
        public Action<T1, T2, T3, T4, T5> commandMethod;

        public Tier5Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier5;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier6Command<T1, T2, T3, T4, T5, T6> : Command
    {
        public Action<T1, T2, T3, T4, T5, T6> commandMethod;

        public Tier6Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier6;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier7Command<T1, T2, T3, T4, T5, T6, T7> : Command
    {
        public Action<T1, T2, T3, T4, T5, T6, T7> commandMethod;

        public Tier7Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier7;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier8Command<T1, T2, T3, T4, T5, T6, T7, T8> : Command
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8> commandMethod;

        public Tier8Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7, T8> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier8;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier9Command<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Command
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> commandMethod;

        public Tier9Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier9;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    [Serializable]
    public class Tier10Command<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Command
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> commandMethod;

        public Tier10Command(string commandOrigin, string commandName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> commandMethod, string commandDescription, string commandSyntax)
        {
            this.commandOrigin = commandOrigin;
            this.commandName = commandName;
            this.commandMethod = CommandMethod(commandMethod);
            this.commandDescription = commandDescription;
            this.commandSyntax = commandSyntax;
            commandTier = CommandTier.Tier10;
        }

        public override void AddCommand(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddCommandHandler(commandName, commandMethod);
            commandState = CommandState.Active;
        }

        public override void RemoveCommand(DialogueRunner dialogueRunner)
        {
            commandState = CommandState.Inactive;
            dialogueRunner.RemoveCommandHandler(commandName);
        }
    }

    #endregion

    public abstract class IRegisterCommands
    {
        public Action CommandMethod(Action action) => action;
        public Action<T1> CommandMethod<T1>(Action<T1> action) => action;
        public Action<T1, T2> CommandMethod<T1, T2>(Action<T1, T2> action) => action;
        public Action<T1, T2, T3> CommandMethod<T1, T2, T3>(Action<T1, T2, T3> action) => action;
        public Action<T1, T2, T3, T4> CommandMethod<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => action;
        public Action<T1, T2, T3, T4, T5> CommandMethod<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => action;
        public Action<T1, T2, T3, T4, T5, T6> CommandMethod<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => action;
        public Action<T1, T2, T3, T4, T5, T6, T7> CommandMethod<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => action;
        public Action<T1, T2, T3, T4, T5, T6, T7, T8> CommandMethod<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => action;
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> CommandMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) => action;
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CommandMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) => action;
    }
}