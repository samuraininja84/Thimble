# Yarn Script Parsing:
There are three attributes called Yarn Node Dropdown, Yarn Speaker Dropdown, and Yarn Command Dropdown.
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
	- This could be good for simply debugging the names in your story, or you could use it as a way to map speakers to certain GameObjects if you're using a custom dialogue system.

- Yarn Variable Dropdown
	- Yarn Variable Dropdown can be used to find any declared variable within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnVariableDropdown("Assets/Resources/Dialogue/Yarn")]
		public string variableName;
		```
	- Intended for use as a helper attribute to get / set values for selected variables from Variable Storage.

- Yarn Command Dropdown
	- Yarn Command Dropdown can be used to find any command within a given file path for use on a string.
	- This can be used like this:
		```csharp
		[YarnCommandDropdown("Assets/Resources/Dialogue/Yarn")]
		public string selectedCommand;
		```
	- I didn't really have a specific use case in mind for this, but I added it anyway. Recommend a use case if you find one.