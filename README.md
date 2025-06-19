# Thimble - A Custom Logging & Debugging System for YarnSpinner

- Purpose:
	- It adds the ability to create custom Commands and Functions that can easily be added to the Yarn Spinner Dialogue Runner and update existing Yarn Variables at runtime.
	- This system's primary purpose is to log the Commands and Functions added to the Dialogue Runner via the AddCommandHandle and Add Function methods, using the Command Center Window and the Function Finder Window, and the Variables added to the In-Memory Variable Storage with the Variable Verifier.
 	- However, there are also a few other utility aspects that should help to make creating stories in Unity using Yarn Spinner easier.

- Dependencies:
	- YarnSpinner for Unity must also be installed in your project

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
		CommandHandler.ActivateCommand(runner, commandData, command);
		```
	- Or like this:
		```csharp
		Tier1Command<string> commandT1 = new Tier1Command<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		Command command = CommandHandler.CreateCommand(commandT1);
		CommandHandler.ActivateCommand(runner, commandData, command);
		```

# Command Creation / Activation Tips:
- The <> brackets denote the type of parameter(s) the command takes in when executed.
- The parameters are passed in the order listed in the brackets.
- If you have a method that takes in no parameters like this: ActivatePlayer();
	- You would use the CreateCommand method like this:
 		```csharp
		Command command = CommandHandler.CreateCommand(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		CommandHandler.ActivateCommand(runner, commandData, command);
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
		CommandHandler.ActivateCommand(runner, commandData, command);
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
		CommandHandler.DeactivateCommand(runner, commandData, command);
		CommandHandler.RemoveCommand(runner, commandData, command);
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

# Minimum Function Set-Up Requirements:
- Add a [SerializeField] Private / Public DialogueRunner to your Script.
- Add a [SerializeField] Private / Public FunctionData to your Script.
	- If you need a new FunctionData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Functions -> New Function Data.
- Add a Private / Public Function Variable to your Script.
	- Pass in the FunctionOrigin, FunctionName, the FunctionMethod, the FunctionDescription, and the FunctionSyntax.
		- The FunctionMethod must use parameters supported by YarnSpinner (No Variables, Float, Int, Bool, String, Game Object, or Component).
			- It supports up to 10 parameters per method, as YarnSpinner does.
		- The FunctionSyntax should be in the format of "<<FunctionName>> or <<FunctionName {Variable}>>" because that is the format that YarnSpinner uses.
- Add the Function using the FunctionHandler.CreateFunction Method in Start or OnEnable.
	- You must pass in the DialogueRunner, the FunctionData, and the Method that will be called when the Function is executed.
- Add the Function to the Dialogue Runner using the FunctionHandler.ActivateFunction Method in Start or OnEnable.
	- You must pass in the DialogueRunner, the FunctionData, and the Function itself;
- Drag in the Dialogue Runner Prefab with the Dialogue Runner Referencer Component into your Scene.
- Press Play and Run your Dialogue until the Function is called.

# Recommended Function Set-Up For Full Use Of The System:
- Create a new Script and add the IFunctionHandle interface to the Class Definition. 
- This will require you to implement the ActivateFunctions & DeactivateFunctions methods. 
- These methods are to be called when you want to Add / Remove a Function to/from a Dialogue Runner. 

- Inside the ActivateFunctions method, you can put any Functions you want to Create & Turn On using the CreateFunction and ActivateFunction Methods.
- You must pass the runner, the Function data, and the method to be called when the Function is executed.
	- You can do so like this:
		```csharp
		Function Function = FunctionHandler.CreateFunction<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		FunctionHandler.ActivateFunction(runner, FunctionData, Function);
		```
	- Or like this:
		```csharp
		Tier1Function<string> FunctionT1 = new Tier1Function<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		Function Function = FunctionHandler.CreateFunction(FunctionT1);
		FunctionHandler.ActivateFunction(runner, FunctionData, Function);
		```

# Function Creation / Activation Tips:
- The <> brackets denote the type of parameter(s) the Function takes in when executed.
- The parameters are passed in the order listed in the brackets.
- If you have a method that takes in no parameters like this: ActivatePlayer();
	- You would use the CreateFunction method like this:
 		```csharp
		Function Function = FunctionHandler.CreateFunction(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		FunctionHandler.ActivateFunction(runner, FunctionData, Function);
   		```
	- Or the Tier0Function method like this:
		```csharp
		Tier0Function FunctionT0 = new Tier0Function(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		Function Function = FunctionHandler.CreateFunction(FunctionT0);
  		```
- If you have a method that takes in multiple parameters like this: SetDetails(string name, int age);
	- You would use the CreateFunction method like this:
		```csharp
		Function Function = FunctionHandler.CreateFunction<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		FunctionHandler.ActivateFunction(runner, FunctionData, Function);
  		```
	- Or a Tier{int}Function method like this:
		```csharp
		Tier2Function<string, int> FunctionT2 = new Tier2Function<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		Function Function = FunctionHandler.CreateFunction(FunctionT2);
 		```
# Function Notes: 
- Regardless of which method you choose, they lead to the same result: A Function takes in a method with a String parameter and sets a name using the input string when the Function is executed within your Yarn Script.
	- The Tier1Function method is just a more specific way of creating a Function to be added to the Dialogue Runner than the CreateFunction method.
	- Behind the scenes, its primary purpose is to supplement the creation of Functions from the FunctionHandler.CreateFunction method.
	- Otherwise, the Tier{int}Function method is used to make it easier to read and understand how many parameters the Function takes in when executed.
		- The int in the Tier{int}Function method denotes the number of parameters the Function takes in when executed.
			- The Tier0Function method is used when the Function takes in no parameters and doesn't need <> brackets.
			- The Tier1Function method is used when the Function takes in one parameter and needs one parameter in the <> brackets.
			- The Tier2Function method is used when the Function takes in two parameters and needs two parameters in the <> brackets.
			- The Tier3Function method is used when the Function takes in three parameters and needs three parameters in the <> brackets.
			- And so on...

- Inside the DeactivateFunctions Method, you can put any Functions you would like to Turn Off using the Deactivate Function or RemoveFunction Methods.
	- You can do so like this:
		```csharp
		FunctionHandler.DeactivateFunction(runner, FunctionData, Function);
		FunctionHandler.RemoveFunction(runner, FunctionData, Function);
		```

# Function Deactivation / Removal Tips: 
- It is recommended to use the DeactivateFunction method to turn off Functions that you may want to turn back on later during Play Mode or when you want to turn off a Function temporarily.
- Use the RemoveFunction method when you are done with the Function and do not need to see it in the tool's logging system, such as when Play Mode has ended. 
- If you want to remove all Functions when Play Mode has ended, put the RemoveFunction method on OnDisable or OnApplicationExit.

# Function Logging:
- You can use the Function Finder Window to see the Functions added to the Dialogue Runner.
- This window will show you all the functions added to the Dialogue Runner and the methods used to execute them.
- Open the Function Finder Window by selecting Tools -> Thimble -> Function Finder.
- Enter Play Mode and open the Function Finder Window. You will see a list of all the Functions added to the Dialogue Runner.
- If it's empty, that may be because you are missing (at least) one of two things: 
	- A Dialogue Runner Referencer on your GameObject that has your Dialogue Runner.
		- If you are missing a Dialogue Runner Referencer, add one by dragging it onto the GameObject with the Dialogue Runner component.
	- A FunctionData ScriptableObject.
		- If you are missing a FunctionData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Functions -> New Function Data.
- By default, there should be a Function Data ScriptableObject in the Runtime/Data folder and a Dialogue Runner Prefab with a Dialogue Runner Referencer component to use as a reference.

# Variable Set-Up Requirements:
- Add a [SerializeField] Private / Public InMemoryVariableStorage to your Script.
- Add a [SerializeField] Private / Public VariableData to your Script.
	- If you need a new VariableData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Variables -> New Variable Data.
- From here, you can use Variable Handler to set and get variables like this:
	- Set Methods:
 		```csharp
 		VariableHandler.SetVariable(InMemoryVariableStorage, VariableData, VariableName, Value) //value = string/float/bool
   		VariableHandler.SetAllVariables(InMemoryVariableStorage storage, VariableData variableData, Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
   		VariableHandler.Clear(InMemoryVariableStorage storage)
   		```
   	- Get Methods:
   	  	```csharp
 		VariableHandler.GetVariable(InMemoryVariableStorage, VariableData, VariableName, out Value) //value = string/float/bool
   		VariableHandler.GetStringVariables(InMemoryVariableStorage storage) // Dictionary<string, string>
   	 	VariableHandler.GetFloatVariables(InMemoryVariableStorage storage) // Dictionary<string, float>
   	  	VariableHandler.GetBoolVariables(InMemoryVariableStorage storage) // Dictionary<string, bool>
   	  	VariableHandler.GetAllVariables(InMemoryVariableStorage storage) // (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>)
   	   	```
# Variable Logging:
- You can use the Variable Verifier Window to see the Variables added to the In-Memory Variable Storage.
- This window will show all the variables added to the In-Memory Variable Storage.
- Open the Variable Verifier Window by selecting Tools -> Thimble -> Variable Verifier.
- Enter Play Mode, open the Variable Verifier Window, and press the Get Variables Button.
- You should see a list of all the Functions added to the In-Memory Variable Storage.
- If it's empty, that may be because you are missing (at least) one of three things: 
	- A Variable Storage Referencer on your GameObject that has your Dialogue Runner.
		- If you are missing a Variable Storage Referencer, add one by dragging it onto the GameObject with the Variable Storage component.
	- A VariableData ScriptableObject.
		- If you are missing a VariableData ScriptableObject, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Variables -> New Variable Data.
- By default, there should be a Variable Data ScriptableObject in the Runtime/Data folder and a Dialogue Runner Prefab with a Variable Storage Referencer component to use as a reference.

# Yarn Script Parsing:
There are three attributes called Yarn Node Dropdown, Yarn Yarn Speaker Dropdown, and Command Dropdown.
- Yarn Node Dropdown
	- Yarn Node Dropdown can be used to select any node within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnNodeDropdown("Assets/Resources/Dialogue/Yarn")]
		public string startNode;
		```
	- Then you can use a Dialogue Runner's Start Node Functionality like with any other string.

- Yarn Speaker Dropdown
	- Yarn Speaker Dropdown can be used to find any command within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnSpeakerDropdown("Assets/Resources/Dialogue/Yarn")]
		public string currentSpeaker;
		```
	- This could be good for simply debugging the names in your story or you could use it as a way to map speaker to certain GameObjects if you're using a custom dialogue system.

- Yarn Command Dropdown
	- Yarn Command Dropdown can be used to find any command within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnCommandDropdown("Assets/Resources/Dialogue/Yarn")]
		public string selectedCommand;
		```
	- I didn't really have a specific use case in mind for this, but I added it anyway. Recommend me a use if you find one for it.
--------------------------------

- That's it! You should now have a custom command system that can be easily added to the Yarn Spinner Dialogue Runner and logged in the Command Editor Window.
- You can also freely modify the system to fit your needs. 
- Feel free to contact me if you have any questions, need help with the system, or are trying to add a new command.
- But for now, I hope you enjoy using the current system and find it helpful for your Yarn Projects.
