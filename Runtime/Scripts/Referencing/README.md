# Dialogue Runner Referencer:
- ```Monobehaviour``` that links your ```CommandData``` and ```FunctionData``` to a specific ```DialogueRunner``` Instance.
- Allow you to register Commands and Functions to the aforementioned ```DialogueRunner``` and any other ones that are registered through the assigned ```CommandData``` and ```FunctionData``` objects.
- Place it onto any ```GameObject``` with a ```DialogueRunner``` attached and assign the ```CommandData``` and/or ```FunctionData```, then it will automatically resolve the references in ```Awake()``` and ```OnDestroy()```

# Variable Storage Referencer:
- ```Monobehaviour``` that links your ```VariableData``` to a specific ```YarnProject``` and ```VariableStorageBehaviour``` Instance.
- Allow you to register Variables to the aforementioned ```YarnProject``` and ```VariableStorageBehaviour```.
- Place it onto any ```GameObject``` with a ```VariableStorageBehaviour``` attached, then it will automatically resolve the references in ```Awake()``` and ```OnDestroy()```
