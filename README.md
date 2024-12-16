# Thimble - A Custom Logging & Debugging System for YarnSpinner

- Purpose:
	- It adds the ability to create custom commands and functions that can easily be added to the Yarn Spinner Dialogue Runner and update existing yarn variables at runtime.
	- But the primary purpose of this system is to add logging for the commands and functions added to the Dialogue Runner via the AddCommandHandle and Add Function method with the Command Center Window and the Function Finder Window and the variables added to the In-Memory Variable Storage.

- Dependencies:
	- YarnSpinner for Unity must also be installed in your project

# Minimum Command Set-Up Requirements:
- Add a [SerializeField] Private / Public DialogueRunner Variable to your Script.
- Add a [SerializeField] Private / Public CommandData Variable to your Script.
	- If you need a new CommandData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Commands -> New Command Data.
- Add a Private / Public Command Variable to your Script.
	- Pass in the CommandOrigin, CommandName, the CommandMethod, the CommandDescription, and the CommandSyntax.
		- The CommandMethod must use parameters supported by YarnSpinner (No Variables, Float, Int, Bool, String, Game Object, or Component).
			- It supports up to 10 parameters per method, as YarnSpinner does.
		- The CommandSyntax should be in the format of "<<CommandName>> or <<CommandName {Variable}>>" because that is the format that YarnSpinner uses.
- Add the Command using the CommandHandler.CreateCommand Method in Start or OnEnable.
- Add the Command to the Dialogue Runner using the CommandHandler.ActivateCommand Method in Start or OnEnable.
	- You need to pass in the DialogueRunner, the CommandData, and the Method that will be called when the command is executed.
- Drag in the Dialogue Runner Prefab with the Dialogue Runner Referencer Component on it into your Scene.
- Press Play and Run your Dialogue until the Command is called.

# Recommended Set-Up For Full Use Of The System:
- Create a new Script and add the ICommandHandler interface to the Class Definition. 
- This will require you to implement the ActivateCommands & DeactivateCommands methods. 
- These methods are to be called when you want to Add / Remove a command to/from a Dialogue Runner. 


- Inside the ActivateCommands method, you can put any commands that you would like to Create & Turn On using the CreateCommand and ActivateCommand Methods.
- You need to pass the runner, the command data, and the method that will be called when the command is executed.
	-You can do so like this:
	```csharp
	- Command command = CommandHandler.CreateCommand<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
	- CommandHandler.ActivateCommand(runner, commandData, command);
	```
	- Or like this:
	```csharp
	- Tier1Command<string> commandT1 = new Tier1Command<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
	- Command command = CommandHandler.CreateCommand(commandT1);
	- CommandHandler.ActivateCommand(runner, commandData, command);
	```

# Creation / Activation Tips:
- The <> brackets denote the type of parameter(s) that the command takes in when it is executed.
- The parameters are passed in the order that they are listed in the brackets.
- If you have a method that takes in no parameters like this: ActivatePlayer();
	- You would use the CreateCommand method like this:
 		```csharp
		- Command command = CommandHandler.CreateCommand(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		- CommandHandler.ActivateCommand(runner, commandData, command);
   		```
	- Or the Tier0Command method like this:
		```csharp
		- Tier0Command commandT0 = new Tier0Command(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		- Command command = CommandHandler.CreateCommand(commandT0);
- If you have a method that takes in multiple parameters like this: SetDetails(string name, int age);
	- You would use the CreateCommand method like this:
		```csharp
		- Command command = CommandHandler.CreateCommand<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		- CommandHandler.ActivateCommand(runner, commandData, command);
	- Or a Tier{int}Command method like this:
		```csharp
		- Tier2Command<string, int> commandT2 = new Tier2Command<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		- Command command = CommandHandler.CreateCommand(commandT2);
 		```
# Notes: 
- Regardless of which method you choose, they lead to the same result: A Command takes in a method with a String parameter and sets a name using the input string when the Command is executed within your Yarn Script.
	- The Tier1Command method is just a more specific way of creating a command to be added to the Dialogue Runner than the CreateCommand method.
	- Behind the scenes, its primary purpose is to supplement the creation of commands from the CommandHandler.CreateCommand method.
	- Otherwise, the Tier{int}Command method is used to make it easier to read and understand how many parameters the command takes in when executed.
		- The int in the Tier{int}Command method denotes the number of parameters the command takes in when executed.
			- The Tier0Command method is used when the command takes in no parameters and doesn't need <> brackets.
			- The Tier1Command method is used when the command takes in one parameter and needs one parameter in the <> brackets.
			- The Tier2Command method is used when the command takes in two parameters and needs two parameters in the <> brackets.
			- The Tier3Command method is used when the command takes in three parameters and needs three parameters in the <> brackets.
			- And so on...

- Inside the DeactivateCommands Method, you can put any commands you would like to Turn Off using the Deactivate Command or RemoveCommand Methods.
	- You can do so like this:
	```csharp
	- CommandHandler.DeactivateCommand(runner, commandData, command);
	- CommandHandler.RemoveCommand(runner, commandData, command);
	```

# Deactivation / Removal Tips: 
- It is recommended to use the DeactivateCommand method to turn off commands that you may want to turn back on later during Play Mode or when you want to turn off a command temporarily.
- Use the RemoveCommand method when you are done with the command and do not need to see it in the tool's logging system, such as when Play Mode has ended. 
- If you want to remove all commands when Play Mode has ended, put the RemoveCommand method on OnDisable or OnApplicationExit.

# Function Set-Up Requirements:
- Add a [SerializeField] Private / Public DialogueRunner Variable to your Script.
- Add a [SerializeField] Private / Public CommandData Variable to your Script.
	- If you need a new CommandData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Commands -> New Command Data.
- Add a Private / Public Command Variable to your Script.
	- Pass in the CommandOrigin, CommandName, the CommandMethod, the CommandDescription, and the CommandSyntax.
		- The CommandMethod must use parameters supported by YarnSpinner (No Variables, Float, Int, Bool, String, Game Object, or Component).
			- It supports up to 10 parameters per method, as YarnSpinner does.
		- The CommandSyntax should be in the format of "<<CommandName>> or <<CommandName {Variable}>>" because that is the format that YarnSpinner uses.
- Add the Command using the CommandHandler.CreateCommand Method in Start or OnEnable.
- Add the Command to the Dialogue Runner using the CommandHandler.ActivateCommand Method in Start or OnEnable.
	- You need to pass in the DialogueRunner, the CommandData, and the Method that will be called when the command is executed.
- Drag in the Dialogue Runner Prefab with the Dialogue Runner Referencer Component on it into your Scene.
- Press Play and Run your Dialogue until the Command is called.

# Recommended Set-Up For Full Use Of The System:
- Create a new Script and add the ICommandHandler interface to the Class Definition. 
- This will require you to implement the ActivateCommands & DeactivateCommands methods. 
- These methods are to be called when you want to Add / Remove a command to/from a Dialogue Runner. 


- Inside the ActivateCommands method, you can put any commands that you would like to Create & Turn On using the CreateCommand and ActivateCommand Methods.
- You need to pass the runner, the command data, and the method that will be called when the command is executed.
	-You can do so like this:
	```csharp
	- Command command = CommandHandler.CreateCommand<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
	- CommandHandler.ActivateCommand(runner, commandData, command);
	```
	- Or like this:
	```csharp
	- Tier1Command<string> commandT1 = new Tier1Command<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
	- Command command = CommandHandler.CreateCommand(commandT1);
	- CommandHandler.ActivateCommand(runner, commandData, command);
	```

# Creation / Activation Tips:
- The <> brackets denote the type of parameter(s) that the command takes in when it is executed.
- The parameters are passed in the order that they are listed in the brackets.
- If you have a method that takes in no parameters like this: ActivatePlayer();
	- You would use the CreateCommand method like this:
 		```csharp
		- Command command = CommandHandler.CreateCommand(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		- CommandHandler.ActivateCommand(runner, commandData, command);
   		```
	- Or the Tier0Command method like this:
		```csharp
		- Tier0Command commandT0 = new Tier0Command(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		- Command command = CommandHandler.CreateCommand(commandT0);
- If you have a method that takes in multiple parameters like this: SetDetails(string name, int age);
	- You would use the CreateCommand method like this:
		```csharp
		- Command command = CommandHandler.CreateCommand<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		- CommandHandler.ActivateCommand(runner, commandData, command);
	- Or a Tier{int}Command method like this:
		```csharp
		- Tier2Command<string, int> commandT2 = new Tier2Command<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		- Command command = CommandHandler.CreateCommand(commandT2);
 		```
# Notes: 
- Regardless of which method you choose, they lead to the same result: A Command takes in a method with a String parameter and sets a name using the input string when the Command is executed within your Yarn Script.
	- The Tier1Command method is just a more specific way of creating a command to be added to the Dialogue Runner than the CreateCommand method.
	- Behind the scenes, its primary purpose is to supplement the creation of commands from the CommandHandler.CreateCommand method.
	- Otherwise, the Tier{int}Command method is used to make it easier to read and understand how many parameters the command takes in when executed.
		- The int in the Tier{int}Command method denotes the number of parameters the command takes in when executed.
			- The Tier0Command method is used when the command takes in no parameters and doesn't need <> brackets.
			- The Tier1Command method is used when the command takes in one parameter and needs one parameter in the <> brackets.
			- The Tier2Command method is used when the command takes in two parameters and needs two parameters in the <> brackets.
			- The Tier3Command method is used when the command takes in three parameters and needs three parameters in the <> brackets.
			- And so on...

- Inside the DeactivateCommands Method, you can put any commands you would like to Turn Off using the Deactivate Command or RemoveCommand Methods.
	- You can do so like this:
	```csharp
	- CommandHandler.DeactivateCommand(runner, commandData, command);
	- CommandHandler.RemoveCommand(runner, commandData, command);
	```

# Deactivation / Removal Tips: 
- It is recommended to use the DeactivateCommand method to turn off commands that you may want to turn back on later during Play Mode or when you want to turn off a command temporarily.
- Use the RemoveCommand method when you are done with the command and do not need to see it in the tool's logging system, such as when Play Mode has ended. 
- If you want to remove all commands when Play Mode has ended, put the RemoveCommand method on OnDisable or OnApplicationExit.

# Logging:
- You can use the Command Editor Window to see the commands added to the Dialogue Runner.
- This window will show you all the commands added to the Dialogue Runner and the methods called when the command is executed.
- To open the Command Editor Window, go to the Tools menu and select the Custom Data Editor.
- Enter Play Mode and open the Command Editor Window. You will see a list of all the commands that have been added to the Dialogue Runner.
- If it's empty, that may be because you are missing (at least) one of two things: 
	- A Dialogue Runner Referencer on your GameObject that has your Dialogue Runner.
		- If you are missing a Dialogue Runner Referencer, add one by dragging it onto the GameObject with the Dialogue Runner component.
	- A CommandData ScriptableObject in your CommandSet.
		- If you are missing a CommandData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Commands -> New Command Data.
		- For logging to work, Put the CommandData ScriptableObject in the CommandSet and ensure it's in the Resources/Data folder.
- By default, there should be a Command Data ScriptableObject in the Resources/Data folder and a Dialogue Runner prefab with a Dialogue Runner Referencer component on it, so you can use those as a reference.

--------------------------------

Thats it! You should now have a custom command system that can be easily added to the Yarn Spinner Dialogue Runner and logged in the Command Editor Window.
You can also freely modify the system to fit your needs. 
Feel free to reach out to me if you have any questions, need help with the system, or are trying to add a new command.
I also plan to look into adding more features to the system to make it more robust and easier to use, such as: 
	- Adding the ability to set a method on a command from the inspector by picking them from a dropdown menu like with the UnityEvent system.
But for now, I hope you enjoy using the current system and find it useful for your Yarn Projects.
