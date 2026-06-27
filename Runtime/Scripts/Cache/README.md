# Yarn Object Cache:
- Thimble now supports caching references to ```GameObject```'s and other custom types via inheritance from ```IYarnObjectCache```. This allows you to easily reference ```GameObject```s and other types in your Yarn scripts without having to manually assign them in the inspector.
- ```IYarnObjectCache``` is an interface that you can implement on any ```MonoBehaviour``` to allow it to be used as a cache for Yarn objects. This means you can create your own custom caches for any type of object you want to reference in your Yarn scripts.
	- Includes:
		```csharp
		string YarnName;
		string ObjectName;
		string ConditionName; //optional, for when you want to have multiple objects with the same YarnName and ObjectName, but different conditions to determine which one to use.
		bool desiredValue; //optional, for when there is a condition name, this is the value that the condition must be for this object to be used.
		string sceneName; //optional, for when there are multiple objects with the same YarnName and ObjectName, but different scenes to determine which one to use.
		bool isLocal; //optional, for when there are multiple objects with the same YarnName and ObjectName, but some are local to the scene and some are global, this can be used to determine which one to use.
		```
- ```YarnObjectReference``` is a ```MonoBehaviour``` that can be used to reference a ```GameObject``` in your Yarn scripts. Inherits from ```IYarnObjectCache``` that it should use to find the object, as well as the ```YarnName``` and ```ObjectName``` that it should look for in the cache.
	- Set the aforementioned properties in the inspector, and then when the ```CachedNameGenerator``` is run it will generate a unique name for this reference based on the ```YarnName``` and ```ObjectName```;
	- It will also handle cases where there are multiple objects with the same ```YarnName``` and ```ObjectName```, but different conditions or scenes to determine which one to use.

#### Usage:
- Set up your ```IYarnObjectCache``` instances.
- Go To 'Tools -> Thimble -> Generate Yarn Object Cache'.
- Click it and pick the save location.
	- (I personally recommend to save it next to the rest of your Yarn Scripts)
- It will loop through every scene in your build settings to find objects with the ```IYarnObjectCache``` interface and start gathering them to serialize to a file.
- You should now have all your ```IYarnObjectCache``` instances filled out as a Yarn Variable and ready for use with automatic referencing based on the condition you've outlined.