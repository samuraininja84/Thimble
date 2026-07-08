# Variable Set-Up Requirements:
- Add a ```[SerializeField] private / public InMemoryVariableStorage``` to your Script.
- Add a ```[SerializeField] private / public VariableData``` to your Script.
	- If you need a new ```VariableData``` ```ScriptableObject```, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Variables -> New Variable Data.
	- This object serves as a singleton so you only need one of them per project.
- From here, you can use ```VariableHandler``` to set and get variables like this:
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
- You can use the ```StoryPinnerWindow``` to see the Variables added to the ```InMemoryVariableStorage```.
	- This window will show all the variables added to the ```InMemoryVariableStorage```.
	- Open the Story Pinner Window by selecting Tools -> Thimble -> Story Pinner.
	- You should see a list of all the Variables added to the ```InMemoryVariableStorage```.
		- If it's empty, that may be because you are missing (at least) one of three things: 
		- A ```VariableStorageReferencer``` on your GameObject that has your Dialogue Runner.
			- If you are missing a ```VariableStorageReferencer```, add one by dragging it onto the GameObject with the Variable Storage component.
		- A ```VariableData``` ```ScriptableObject```.
			- If you are missing a ```VariableData``` ```ScriptableObject```, you can create one by right-clicking in the Project window and selecting Create -> Thimble -> Variables -> New Variable Data.
	- By default, there should be a ```VariableData``` ```ScriptableObject``` in the Runtime/Data folder and a Dialogue Runner Prefab with a ```VariableStorageReferencer``` component to use as a reference.

# Variable References:
- There are ```struct```s for each of the Primitive Types that Yarn Variables can be serialized to with custom property drawers linked to your project's ```Variable Data```.
	- ```StringVariable```
	- ```FloatVariable```
	- ```BoolVariable```
- As well as composite versions of each of the above ```struct```s with multiple Yarn Variable inputs:
	- ```CompositeStringVariable```
	- ```CompositeFloatVariable```
	- ```CompositeBoolVariable```
- They all inherit from ```IVariable<T>``` and each of these can be used to validate Yarn Variables at runtime and to change the value of their underlying Yarn Variable(s) in Yarn Spinner with field & methods like:
	- ```IVariable<T>.Name```
	- ```IVariable<T>Value```
	- ```IVariable<T>.GetName()```
	- ```IVariable<T>.GetValue()```