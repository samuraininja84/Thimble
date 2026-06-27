# Thimble - A Custom Logging & Debugging System for Yarn Spinner

#### Purpose:
- It adds the ability to create custom Commands and Functions that can easily be added to the Yarn Spinner ```DialogueRunner``` and update existing Yarn Variables at runtime.
- This system's primary purpose is logging and debugging the:
	- Commands and Functions added to the ```DialogueRunner``` via the ```AddCommand()``` and ```AddFunction()``` methods, using the ```StoryPinnerWindow```.
	- Variables added to the ```InMemoryVariableStorage``` with the ```StoryPinnerWindow```.
- However, there are also a few other utility aspects that should help to make creating stories in Unity using Yarn Spinner easier.

#### Dependencies:
- Yarn Spinner for Unity must also be installed in your project.
	- Currently only actively supporting Yarn Spinner 3+ versions.
	- Functionality should still work on older versions but minor error may still be thrown upon installation.

#### Documentation
- [Attributes](Runtime/Scripts/Attributes/README.md)
- [Cache](Runtime/Scripts/Cache/README.md)
- [Commands](Runtime/Scripts/Commands/README.md)
- [Functions](Runtime/Scripts/Functions/README.md)
- [Variables](Runtime/Scripts/Variables/README.md)
- [Referencing](Runtime/Scripts/Referencing/README.md)
- [Interfaces](Runtime/Scripts/Interfaces/README.md)

--------------------------------

#### Other Notes
- You can also freely modify the system to fit your needs. 
	- You also may log an issue for me to fix if you find any.
- Feel free to contact me if you have any questions or need help with the system.
	- But for now, I hope you enjoy using the current system and find it helpful for your Yarn Projects.
