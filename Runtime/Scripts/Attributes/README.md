# Yarn Script Parsing:
There are four attributes called ```YarnNodeDropdown```, ```YarnSpeakerDropdown```, ```YarnVariableDropdown```, and ```YarnCommandDropdown```.
- ```YarnNodeDropdown```
	- ```YarnNodeDropdown``` can be used to select any node within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnNodeDropdown("Assets/Resources/Dialogue/Yarn")]
		public string startNode;
		```
	- Then you can use a ```DialogueRunner```'s ```StartNode()``` method like with any other string.
- ```YarnSpeakerDropdown```
	- ```YarnSpeakerDropdown``` can be used to find any speaker within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnSpeakerDropdown("Assets/Resources/Dialogue/Yarn")]
		public string currentSpeaker;
		```
	- This could be good for simply debugging the names in your story, or you could use it as a way to map speakers to certain GameObjects if you're using a custom dialogue system.
- ```YarnVariableDropdown```
	- ```YarnVariableDropdown``` can be used to find any declared variable within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnVariableDropdown("Assets/Resources/Dialogue/Yarn")]
		public string variableName;
		```
	- Intended for use as a helper attribute to get / set values for selected variables from ```VariableData``` or its associated ```VariableStorageBehaviour```.
- ```YarnCommandDropdown```
	- ```YarnCommandDropdown``` can be used to find any command within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnCommandDropdown("Assets/Resources/Dialogue/Yarn")]
		public string selectedCommand;
		```
	- I didn't really have a specific use case in mind for this, but I added it anyway. 
	- Recommend a use case if you find one.