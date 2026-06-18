# Minimum Command Set-Up Requirements:
- Add a [SerializeField] Private / Public DialogueRunner to your Script.
- Add a [SerializeField] Private / Public CommandData to your Script.
	- If you need a new CommandData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Commands -> New Command Data.
- Add a Private / Public Command to your Script.
	- Pass in the CommandOrigin, CommandName, the CommandMethod, the CommandDescription, and the CommandSyntax.
		The CommandMethod must use parameters supported by YarnSpinner (No Variables, Floats, Ints, Bools, Strings, Game Objects, or Components).
			- It supports up to 10 parameters per method, as YarnSpinner does.
		- The CommandSyntax should be in the format of "<<CommandName>> or <<CommandName {Variable}>>" because that is the format that YarnSpinner uses.
- Add the Command using the CommandHandler.CreateCommand Method in Start or OnEnable.
	- You need to pass in the DialogueRunner, the CommandData, and the Method that will be called when the Command is executed.
- Add the Command to the Dialogue Runner using the CommandHandler.ActivateCommand Method in Start or OnEnable.
	- You must pass in the DialogueRunner, the CommandData, and the Command itself.
- Drag in the Dialogue Runner Prefab with the Dialogue Runner Referencer Component into your Scene.
- Press Play and Run your Dialogue until the Command is called.

# Recommended Command Set-Up For Full Use Of The System:
- Create a new Script and add the ICommandHandle interface to the Class Definition. 
- This will require you to implement the ActivateCommands & DeactivateCommands methods. 
- These methods are to be called when you want to Add / Remove a command to/from a Dialogue Runner. 

- Inside the ActivateCommands method, you can put any commands you would like to Create & Turn On using the CreateCommand and ActivateCommand Methods.
- You must pass the runner, the command data, and the method to be called when executing the command.
	- You can do so like this:
		```csharp
		Command command = CommandHandler.CreateCommand<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		CommandHandler.ActivateCommand(commandData, command, runner);
		```
	- Or like this:
		```csharp
		Tier1Command<string> commandT1 = new Tier1Command<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		Command command = CommandHandler.CreateCommand(commandT1);
		CommandHandler.ActivateCommand(commandData, command, runner);
		```

# Command Creation / Activation Tips:
- The <> brackets denote the type of parameter(s) the command takes in when executed.
- The parameters are passed in the order listed in the brackets.
- If you have a method that takes in no parameters like this: ActivatePlayer();
	- You would use the CreateCommand method like this:
 		```csharp
		Command command = CommandHandler.CreateCommand(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		CommandHandler.ActivateCommand(commandData, command, runner);
   		```
	- Or the Tier0Command method like this:
		```csharp
		Tier0Command commandT0 = new Tier0Command(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		Command command = CommandHandler.CreateCommand(commandT0);
  		```
- If you have a method that takes in multiple parameters like this: SetDetails(string name, int age);
	- You would use the CreateCommand method like this:
		```csharp
		Command command = CommandHandler.CreateCommand<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		CommandHandler.ActivateCommand(commandData, command, runner);
  		```
	- Or a Tier{int}Command method like this:
		```csharp
		Tier2Command<string, int> commandT2 = new Tier2Command<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		Command command = CommandHandler.CreateCommand(commandT2);
 		```
# Command Notes: 
- Regardless of your chosen method, they lead to the same result: A Command takes in a method with a String parameter and sets a name using the input string when the Command is executed within your Yarn Script.
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
		CommandHandler.DeactivateCommand(commandData, command, runner);
		CommandHandler.RemoveCommand(commandData, command, runner);
		```

# Command Deactivation / Removal Tips: 
- It is recommended to use the DeactivateCommand method to turn off commands that you may want to turn back on later during Play Mode or when you want to turn off a command temporarily.
- Use the RemoveCommand method when you are done with the command and do not need to see it in the tool's logging system, such as when Play Mode has ended. 
- If you want to remove all commands when Play Mode has ended, put the RemoveCommand method on OnDisable or OnApplicationExit.

# Command Logging:
- You can use the Command Center Window to see the Commands added to the Dialogue Runner.
- This window will show you all the Commands added to the Dialogue Runner and the methods used to execute them.
- Open the Command Center Window by selecting Tools -> Thimble -> Command Center.
- Enter Play Mode and open the Command Center Window. You will see a list of all the Commands added to the Dialogue Runner.
- If it's empty, that may be because you are missing (at least) one of two things: 
	- A Dialogue Runner Referencer on your GameObject that has your Dialogue Runner.
		- If you are missing a Dialogue Runner Referencer, add one by dragging it onto the GameObject with the Dialogue Runner component.
	- A CommandData ScriptableObject.
		- If you are missing a CommandData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Commands -> New Command Data.
- By default, there should be a Command Data ScriptableObject in the Runtime/Data folder and a Dialogue Runner Prefab with a Dialogue Runner Referencer component to use as a reference.