# Minimum Function Set-Up Requirements:
- Add a ```[SerializeField] private / public DialogueRunner``` to your Script.
- Add a ```[SerializeField] private / public FunctionData``` to your Script.
	- If you need a new ```FunctionData``` ```ScriptableObject```, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Functions -> New Function Data.
- Add a ```private / public FunctionVariable``` to your Script.
	- Pass in the ```FunctionOrigin```, ```FunctionName```, the ```FunctionMethod```, the ```FunctionDescription```, and the ```FunctionSyntax```.
		- The ```FunctionMethod``` must use parameters supported by Yarn Spinner (No Variables, ```float```, ```int```, ```bool```, ```string```, ```GameObject```, or ```Component```).
			- It supports up to 10 parameters per method, as Yarn Spinner does.
		- The ```FunctionSyntax``` should be in the format of "```<<FunctionName>>``` or ```<<FunctionName {Variable}>>```" because that is the format that Yarn Spinner uses.
- Add the Function using the ```FunctionHandler.CreateFunction()``` method in ```Start()``` or ```OnEnable()```.
	- You must pass in the ```DialogueRunner```, the ```FunctionData```, and the Method that will be called when the Function is executed.
- Add the Function to the ```DialogueRunner``` using the ```FunctionHandler```.```ActivateFunction()``` method in ```Start()``` or ```OnEnable()```.
	- You must pass in the ```DialogueRunner```, the ```FunctionData```, and the Function itself;
- Drag in the ```DialogueRunner``` Prefab with the ```DialogueRunnerReferencer``` Component into your Scene.
- Press Play and Run your Dialogue until the Function is called.

# Recommended Function Set-Up For Full Use Of The System:
- Create a new script and add the ```IFunctionHandle``` ```interface``` to the ```class``` Definition. 
- This will require you to implement the ```ActivateFunctions()``` & ```DeactivateFunctions()``` methods. 
- These methods are to be called when you want to Add / Remove a Function to/from a Dialogue Runner. 
- Inside the ```ActivateFunctions()``` method, you can put any Functions you want to create and activate using the ```CreateFunction()``` and ```ActivateFunction()``` methods.
- You must pass the ```DialogueRunner```, the ```FunctionData```, and the method to be called when the Function is executed.
	- You can do so like this:
		```csharp
		Function Function = FunctionHandler.CreateFunction<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		FunctionHandler.ActivateFunction(FunctionData, Function, runner);
		```
	- Or like this:
		```csharp
		Tier1Function<string> FunctionT1 = new Tier1Function<string>(name, "setName", SetName, "Sets the player's name", "<<setName {name}>>");
		Function Function = FunctionHandler.CreateFunction(FunctionT1);
		FunctionHandler.ActivateFunction(FunctionData, Function, runner);
		```

# Function Creation / Activation Tips:
- The <> brackets denote the type of parameter(s) the Function takes in when executed.
- The parameters are passed in the order listed in the brackets.
- If you have a method that takes in no parameters like this: ```ActivatePlayer()```;
	- You would use the ```FunctionHandler.CreateFunction()``` method like this:
 		```csharp
		Function Function = FunctionHandler.CreateFunction(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		FunctionHandler.ActivateFunction(FunctionData, Function, runner);
   		```
	- Or the ```Tier0Function``` ```class``` like this:
		```csharp
		Tier0Function FunctionT0 = new Tier0Function(name, "activatePlayer", ActivatePlayer, "Activates the player", "<<activatePlayer>>");
		Function Function = FunctionHandler.CreateFunction(FunctionT0);
  		```
- If you have a method that takes in multiple parameters like this: ```SetDetails(string name, int age)```;
	- You would use the ```FunctionHandler.CreateFunction()``` method like this:
		```csharp
		Function Function = FunctionHandler.CreateFunction<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		FunctionHandler.ActivateFunction(FunctionData, Function, runner);
  		```
	- Or a ```Tier{int}Function``` ```class``` like this:
		```csharp
		Tier2Function<string, int> FunctionT2 = new Tier2Function<string, int>(name, "setDetails", SetDetails, "Sets the player's name and age", "<<setDetails {name} {age}>>");
		Function Function = FunctionHandler.CreateFunction(FunctionT2);
 		```
# Function Notes: 
- Regardless of which method you choose, they lead to the same result: 
	- A Function that takes in a method with a ```string``` parameter and sets a name using the input string when the Function is executed within your Yarn Script.
	- The ```Tier{int}Function``` ```class``` is just a more specific way of creating a Function to be added to the ```DialogueRunner``` than the ```FunctionHandler.CreateFunction()``` method.
	- Behind the scenes, its primary purpose is to supplement the creation of Functions from the ```FunctionHandler.CreateFunction()``` method.
	- Otherwise, the ```Tier{int}Function``` ```class``` is used to make it easier to read and understand how many parameters the Function takes in when executed.
		- The ```int``` in the ```Tier{int}Function``` ```class``` denotes the number of parameters the Function takes in when executed.
			- The ```Tier0Function``` ```class``` is used when the Function takes in no parameters and doesn't need <> brackets.
			- The ```Tier1Function``` ```class``` is used when the Function takes in one parameter and needs one parameter in the <> brackets.
			- The ```Tier2Function``` ```class``` is used when the Function takes in two parameters and needs two parameters in the <> brackets.
			- The ```Tier3Function``` ```class``` is used when the Function takes in three parameters and needs three parameters in the <> brackets.
			- And so on...

- Inside the ```DeactivateFunctions()``` method, you can put any Functions you would like to turn off using the ```DeactivateFunction()``` or ```RemoveFunction()``` methods.
	- You can do so like this:
		```csharp
		FunctionHandler.DeactivateFunction(FunctionData, Function, runner);
		FunctionHandler.RemoveFunction(FunctionData, Function, runner);
		```

# Function Deactivation / Removal Tips: 
- It is recommended to use the ```DeactivateFunction()``` method to turn off Functions that you may want to turn back on later during Play Mode or when you want to turn off a Function temporarily.
- Use the ```RemoveFunction()``` method when you are done with the Function and do not need to see it in the tool's logging system, such as when Play Mode has ended. 
- If you want to remove all Functions when Play Mode has ended, put the ```RemoveFunction()``` method on ```OnDisable()``` or ```OnApplicationExit()```.

# Function Logging:
- You can use the ```StoryPinnerWindow``` to see the Functions added to the ```DialogueRunner```.
- This window will show you all the functions added to the ```DialogueRunner``` and the methods used to execute them.
- Open the ```StoryPinnerWindow``` by selecting Tools -> Thimble -> Story Pinner.
- Enter Play Mode and open the ```StoryPinnerWindow```. You will see a list of all the Functions added to the ```DialogueRunner```.
- If it's empty, that may be because you are missing (at least) one of two things: 
	- A ```DialogueRunnerReferencer``` on your GameObject that has your ```DialogueRunner```.
		- If you are missing a ```DialogueRunnerReferencer```, add one by dragging it onto the GameObject with the ```DialogueRunner``` component.
	- A ````FunctionData``` ```ScriptableObject```.
		- If you are missing a ````FunctionData``` ```ScriptableObject```, you can create one by right-clicking in the Project window and selecting Create -> Functions -> New Function Data.
- By default, there should be a ```FunctionData``` ```ScriptableObject``` in the Runtime/Data folder and a ```DialogueRunner``` Prefab with a ```DialogueRunnerReferencer``` component to use as a reference.