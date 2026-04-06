using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Variable Data", menuName = "Thimble/Variables/New Variable Data")]
    public class VariableData : ScriptableObject
    {
        [Header("Yarn Spinner")]
        public YarnProject yarnProject;
        public VariableStorageBehaviour storage;
        public bool filterInternalVariables = false;

        [Header("String Variables")]
        public SerializableDictionary<string, string> stringVariables = new();

        [Header("Float Variables")]
        public SerializableDictionary<string, float> floatVariables = new();

        [Header("Bool Variables")]
        public SerializableDictionary<string, bool> boolVariables = new();

        public static Action OnVariableUpdate;
        public static Action<string, string> OnStringVariableChange;
        public static Action<string, float> OnFloatVariableChange;
        public static Action<string, bool> OnBoolVariableChange;

        protected static VariableData instance;

        /// <summary>
        /// The singleton instance of the <see cref="VariableData"/>. 
        /// </summary>
        /// <remarks>
        /// This property provides access to the single instance of the <see cref="VariableData"/> class, ensuring that only one instance exists throughout the application. 
        /// If an instance does not already exist, it attempts to find and load one from the project's assets. 
        /// If no instance is found, it will return null until an instance is created or assigned.
        /// </remarks>
        public static VariableData Instance
        {
            get
            {
#if UNITY_EDITOR
                // Only look for an instance if there isn't one already assigned, to avoid unnecessary searches
                if (!HasInstance)
                {
                    // Search the project for assets of type VariableData and get their GUIDs
                    var guids = UnityEditor.AssetDatabase.FindAssets("t:" + nameof(VariableData));

                    // If at least one VariableData asset is found, load the first one
                    if (guids.Length > 0) instance = (VariableData)UnityEditor.AssetDatabase.LoadMainAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]));
                }
#endif
                // Return the instance
                return instance;
            }
        }

        /// <summary>
        /// A boolean property that indicates whether an instance of the <see cref="VariableData"/> exists.
        /// </summary>
        public static bool HasInstance => instance != null;

        private void OnEnable()
        {
            // If there is no existing instance, assign this instance to the static instance variable.
            if (!HasInstance) instance = this;
        }

        #region Reference Setters

        public void SetProject(YarnProject project) => yarnProject = project;

        public void SetStorage(VariableStorageBehaviour storage) => this.storage = storage;

        #endregion

        #region Listener Management

        public void AddChangeListener(Action<string, object> onChange) => storage.AddChangeListener(onChange);

        public void AddChangeListener<T>(string variableName, Action<T> onChange) => storage.AddChangeListener(variableName, onChange);

        public void AddUpdateListener(Action onUpdate) => OnVariableUpdate += onUpdate;

        public void RemoveUpdateListener(Action onUpdate) => OnVariableUpdate -= onUpdate;

        public void AddStringListener(Action<string, string> onChange) => OnStringVariableChange += onChange;

        public void RemoveStringListener(Action<string, string> onChange) => OnStringVariableChange -= onChange;

        public void AddFloatListener(Action<string, float> onChange) => OnFloatVariableChange += onChange;

        public void RemoveFloatListener(Action<string, float> onChange) => OnFloatVariableChange -= onChange;

        public void AddBoolListener(Action<string, bool> onChange) => OnBoolVariableChange += onChange;

        public void RemoveBoolListener(Action<string, bool> onChange) => OnBoolVariableChange -= onChange;

        #endregion

        #region Invoke Listeners

        public void InvokeVariableUpdate() => OnVariableUpdate?.Invoke();

        public void InvokeStringChange(string name, string value) => OnStringVariableChange?.Invoke(name, value);

        public void InvokeFloatChange(string name, float value) => OnFloatVariableChange?.Invoke(name, value);

        public void InvokeBoolChange(string name, bool value) => OnBoolVariableChange?.Invoke(name, value);

        #endregion

        #region Variable Management

        [ContextMenu("Initialize Variables")]
        public void Initialize()
        {
            // If the project is null, log an error and return
            if (yarnProject == null)
            {
                // Log an error if there is no Yarn Project
                Debug.LogError("Yarn Project is not set. Please set the Yarn Project before initializing variables.");

                // Return early if there is no Yarn Project
                return;
            }

            // Clear all variable lists before initializing
            Clear();

            // Iterate through the variable data and populate the dictionaries based on the variable type
            foreach (var pair in yarnProject.InitialValues)
            {
                // Get the key and value from the pair
                var key = pair.Key;
                var value = pair.Value;

                // If the filter internal variables option is enabled and the key contains the internal variable denotator, skip this variable
                if (filterInternalVariables && key.Contains(VariableHandler.InternalVariableDenotator, StringComparison.OrdinalIgnoreCase)) continue;

                // Check the type of the value and add it to the appropriate dictionary
                switch (value)
                {
                    case string stringValue:
                        CreateVariable(key, stringValue);
                        break;
                    case float floatValue:
                        CreateVariable(key, floatValue);
                        break;
                    case bool boolValue:
                        CreateVariable(key, boolValue);
                        break;
                }
            }

            // Sort the variables after initializing
            UpdateVariables();
        }

        [ContextMenu("Sort Variables")]
        public void SortVariables()
        {
            // Sort the string, float, and bool variables by their names
            stringVariables.OrderBy(variable => variable.Key);
            floatVariables.OrderBy(variable => variable.Key);
            boolVariables.OrderBy(variable => variable.Key);
        }

        [ContextMenu("Update Variables")]
        public void UpdateVariables()
        {
            // If the storage is null, log an error and return
            if (storage == null)
            {
                // Log a warning if there is no variable storage
                Debug.LogWarning("Variable storage is not set. Please set the variable storage before updating variables.");

                // Return early if there is no variable storage
                return;
            }

            // Add all the string variables to the dictionaries
            foreach (var kvp in stringVariables) storage.SetValue(kvp.Key, kvp.Value);

            // Add all the float variables to the dictionaries
            foreach (var kvp in floatVariables) storage.SetValue(kvp.Key, kvp.Value);

            // Add all the bool variables to the dictionaries
            foreach (var kvp in boolVariables) storage.SetValue(kvp.Key, kvp.Value);
        }

        [ContextMenu("Refresh Variables")]
        public void RefreshVariables()
        {
            // Clear all variable lists before getting new variables
            Clear();

            // Get all variables types from the storage
            GetStringVariables();
            GetFloatVariables();
            GetBoolVariables();

            // Sort the variables after getting them
            SortVariables();
        }

        [ContextMenu("Clear Variables")]
        public void Clear()
        {
            // Clear all variable lists
            stringVariables.Clear();
            floatVariables.Clear();
            boolVariables.Clear();
        }

        public bool Equal()
        {
            // Initialize the equal variable to true
            bool equal = true;

            // Check if the string variables are equal to the storage
            foreach (var variable in stringVariables)
            {
                // Try to get the value of the variable from the storage and compare it to the variable value
                if (storage.TryGetValue(variable.Key, out string value))
                {
                    // If the variable value is not equal to the value in the storage, log an error and set equal to false
                    if (!value.Equals(variable.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        // Set equal to false if the variable value is not equal to the value in the storage
                        equal = false;

                        // If the variable value is not equal to the value in the storage, log an error and return false
                        return equal;
                    }
                }
                else
                {
                    // If the variable does not exist in the variable storage, log an error and set equal to false
                    equal = false;

                    // If the variable does not exist in the variable storage, log an error and return false
                    return equal;
                }
            }

            // Check if the float variables are equal to the storage
            foreach (var variable in floatVariables)
            {
                // Try to get the value of the variable from the storage and compare it to the variable value
                if (storage.TryGetValue(variable.Key, out float value))
                {
                    // If the variable value is not approximately equal to the value in the storage, log an error and set equal to false
                    if (!variable.Value.Approximately(value))
                    {
                        // Set equal to false if the variable value is not approximately equal to the value in the storage
                        equal = false;

                        // If the variable value is not approximately equal to the value in the storage, log an error and return false
                        return equal;
                    }
                }
                else
                {
                    // If the variable does not exist in the variable storage, log an error and set equal to false
                    equal = false;

                    // If the variable does not exist in the variable storage, log an error and return false
                    return equal;
                }
            }

            // Check if the bool variables are equal to the storage
            foreach (var variable in boolVariables)
            {
                // Try to get the value of the variable from the storage and compare it to the variable value
                if (storage.TryGetValue(variable.Key, out bool value))
                {
                    // If the variable value is not equal to the value in the storage, log an error and set equal to false
                    if (variable.Value != value)
                    {
                        // Set equal to false if the variable value is not equal to the value in the storage
                        equal = false;

                        // If the variable value is not equal to the value in the storage, log an error and return false
                        return equal;
                    }
                }
                else
                {
                    // If the variable does not exist in the variable storage, log an error and set equal to false
                    equal = false;

                    // If the variable does not exist in the variable storage, log an error and return false
                    return equal;
                }
            }

            // Return the equal variable
            return equal;
        }

        public bool Empty() => stringVariables.Count == 0 && floatVariables.Count == 0 && boolVariables.Count == 0;

        #endregion

        #region Create Variable Methods

        public void CreateVariable(string name, string value) => stringVariables.Add(name, value);

        public void CreateVariable(string name, float value) => floatVariables.Add(name, value);

        public void CreateVariable(string name, bool value) => boolVariables.Add(name, value);

        #endregion

        #region Set Variable Methods

        public void SetValue(string name, string value)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the storage is not null, set the value in the storage
            if (storage != null)
            {
                // If the variable does not exist in the variable storage, log an error and return
                if (!storage.Contains(name))
                {
                    // Log an error if the variable does not exist in the variable storage
                    Debug.LogError("Variable " + name + " does not exist in the variable storage.");

                    // Return early if the variable does not exist in the variable storage
                    return;
                }

                // Set the value of the variable
                storage.SetValue(name, value);
            }

            // Find the variable in the variables list and update the value
            stringVariables[name] = value;

            // Invoke the string variable change event after setting the value
            InvokeStringChange(name, value);

            // Invoke the variable update event after setting all variables
            InvokeVariableUpdate();
        }

        public void SetValue(string name, float value)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the storage is not null, set the value in the storage
            if (storage != null)
            {
                // If the variable does not exist in the variable storage, log an error and return
                if (!storage.Contains(name))
                {
                    // Log an error if the variable does not exist in the variable storage
                    Debug.LogError("Variable " + name + " does not exist in the variable storage.");

                    // Return early if the variable does not exist in the variable storage
                    return;
                }

                // Set the value of the variable
                storage.SetValue(name, value);
            }

            // Find the variable in the variables list and update the value
            floatVariables[name] = value;

            // Invoke the float variable change event after setting the value
            InvokeFloatChange(name, value);

            // Invoke the variable update event after setting all variables
            InvokeVariableUpdate();
        }

        public void SetValue(string name, bool value)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the storage is not null, set the value in the storage
            if (storage != null)
            {
                // If the variable does not exist in the variable storage, log an error and return
                if (!storage.Contains(name))
                {
                    // Log an error if the variable does not exist in the variable storage
                    Debug.LogError("Variable " + name + " does not exist in the variable storage.");

                    // Return early if the variable does not exist in the variable storage
                    return;
                }

                // Set the value of the variable
                storage.SetValue(name, value);
            }

            // Find the variable in the variables list and update the value
            boolVariables[name] = value;

            // Invoke the bool variable change event after setting the value
            InvokeBoolChange(name, value);

            // Invoke the variable update event after setting all variables
            InvokeVariableUpdate();
        }

        public void SetAllVariables(Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            // Set all string variables in the storage
            storage.SetAllVariables(floatVariables, stringVariables, boolVariables);

            // Invoke the variable update event after setting all variables
            InvokeVariableUpdate();
        }

        #endregion

        #region Get Variable Methods

        public void GetStringVariables() => stringVariables = storage.GetAllVariables().Item2.ConvertTo();

        public void GetFloatVariables() => floatVariables = storage.GetAllVariables().Item1.ConvertTo();

        public void GetBoolVariables() => boolVariables = storage.GetAllVariables().Item3.ConvertTo();

        public string GetVariable(string name, out string value)
        {
            // Initialize the out value
            value = string.Empty;

            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the storage is null, check the string variables list
            if (storage == null)
            {
                // Check the float variables list for the variable name first
                foreach (var variable in stringVariables)
                {
                    // If the variable name matches, get the string value
                    if (variable.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Get the string value from the variable
                        value = variable.Value;

                        // Return the string value
                        return value;
                    }
                }
            }
            else
            {
                // If the variable does not exist in the variable storage, log an error
                if (!storage.TryGetValue(name, out value)) Debug.LogError("Variable: " + name + " does not exist in the variable storage.");

                // Return the default float value
                return value;
            }

            // Return the value of the variable
            return value;
        }

        public float GetVariable(string name, out float value)
        {
            // Initialize the out value
            value = 0f;

            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the storage is null, check the float variables list
            if (storage == null)
            {
                // Check the float variables list for the variable name first
                foreach (var variable in floatVariables)
                {
                    // If the variable name matches, get the float value
                    if (variable.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Get the float value from the variable
                        value = variable.Value;

                        // Return the float value
                        return value;
                    }
                }
            }
            else
            {
                // If the variable does not exist in the variable storage, log an error
                if (!storage.TryGetValue(name, out value)) Debug.LogError("Variable: " + name + " does not exist in the variable storage.");

                // Return the default float value
                return value;
            }

            // Return the value of the variable
            return value;
        }

        public bool GetVariable(string name, out bool value)
        {
            // Initialize the out value
            value = false;

            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the storage is null, check the bool variables list
            if (storage == null)
            {
                // Check the bool variables list for the variable name first
                foreach (var variable in boolVariables)
                {
                    // If the variable name matches, get the bool value
                    if (variable.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Get the bool value from the variable
                        value = variable.Value;

                        // Return the bool value
                        return value;
                    }
                }
            }
            else             
            {
                // If the variable does not exist in the variable storage, log an error
                if (!storage.TryGetValue(name, out value)) Debug.LogError("Variable: " + name + " does not exist in the variable storage.");

                // Return the default float value
                return value;
            }

            // Return the value of the variable
            return value;
        }

        public Type GetVariableType(string name)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!name.StartsWith("$")) name = "$" + name;

            // If the variable does not exist in the variable storage, log an error and return null
            if (!storage.Contains(name))
            {
                // Log an error if the variable does not exist in the variable storage
                Debug.LogError("Variable: " + name + " does not exist in the variable storage.");

                // Return null if the variable does not exist in the variable storage
                return null;
            }

            // Try to get the type of the variable
            if (storage.TryGetValue(name, out string stringValue)) return typeof(string);
            else if (storage.TryGetValue(name, out float floatValue)) return typeof(float);
            else if (storage.TryGetValue(name, out bool boolValue)) return typeof(bool);

            // If the variable type is not found, log an error
            Debug.LogError("Variable type for: " + name + " not found in the variable storage.");

            // Return null if the variable type is not found
            return null;
        }

        public (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables() => storage.GetAllVariables();

        #endregion
    }
}
