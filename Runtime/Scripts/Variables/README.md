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