# ```ICommandHandle```:
- ```interface``` that enforces the use of two voids ```ActivateCommands()``` and ```DeactivateCommands()``` for use in Command registration.
- Not required for any functionality, but it is useful for searching for any scripts that can add Commands to your ```DialogueRunner```(s).

# ```IFunctionHandle```:
- ```interface``` that enforces the use of two voids ```ActivateFunctions()``` and ```DeactivateFunctions()``` for use in Function registration.
- Not required for any functionality, but it is useful for searching for any scripts that can add Functions to your ```DialogueRunner```(s).

#### Usage:
- Add the ```interface```s to any class that you wish to handle Commands or Functions.
    - Activate and Deactivate them via a method like this:
        ```
            private void OnEnable() => ActivateCommands();

            private void OnDisable() => DeactivateCommands();

            public virtual void ActivateCommands()
            {
                // Create a new command for the TransitionIn method
                commands.Add(CommandHandler.CreateCommand<string>(name, "transitionIn", TransitionIn, "Starts a Transition by name", "<<transitionIn {name}>>"));

                // Create a new command for the TransitionOut method
                commands.Add(CommandHandler.CreateCommand<string>(name, "transitionOut", TransitionOut, "Ends a Transition by name", "<<transitionOut {name}>>"));

                // Actiavte the commands from the command handler
                commands.ForEach(command => CommandHandler.ActivateCommand(commandData, command, runner));
            }

            public virtual void DeactivateCommands() => commands.ForEach(command => CommandHandler.RemoveCommand(commandData, command));
        ```