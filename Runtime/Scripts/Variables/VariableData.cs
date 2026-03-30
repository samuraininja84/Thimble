using System;
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
        public bool filterInternalVariables = true;

        [Header("String Variables")]
        public List<Variable> stringVariables = new();

        [Header("Float Variables")]
        public List<Variable> floatVariables = new();

        [Header("Bool Variables")]
        public List<Variable> boolVariables = new();

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

        public void SetProject(YarnProject project) => yarnProject = project;

        public void SetStorage(VariableStorageBehaviour storage) => this.storage = storage;

        public void AddChangeListener(Action<string, object> onChange) => storage.AddChangeListener(onChange);

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

            // If the storage is not null, update the variables in the storage
            if (storage != null)
            {
                // Add all the string variables to the dictionaries
                stringVariables.ForEach(variable => storage.SetValue(variable.GetName(), variable.stringValue));

                // Add all the float variables to the dictionaries
                floatVariables.ForEach(variable => storage.SetValue(variable.GetName(), variable.floatValue));

                // Add all the bool variables to the dictionaries
                boolVariables.ForEach(variable => storage.SetValue(variable.GetName(), variable.boolValue));
            }
        }

        [ContextMenu("Sort Variables")]
        public void SortVariables()
        {
            // Sort the string, float, and bool variables by their names
            stringVariables.Sort((x, y) => x.Name.CompareTo(y.Name));
            floatVariables.Sort((x, y) => x.Name.CompareTo(y.Name));
            boolVariables.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        [ContextMenu("Update Variables")]
        public void UpdateVariables()
        {
            // If the storage is null, log an error and return
            if (storage == null)
            {
                // Log an error if there is no variable storage
                Debug.LogError("Variable storage is not set. Please set the variable storage before updating variables.");

                // Return early if there is no variable storage
                return;
            }

            // Add all the string variables to the dictionaries
            stringVariables.ForEach(variable => storage.SetValue(variable.GetName(), variable.stringValue));

            // Add all the float variables to the dictionaries
            floatVariables.ForEach(variable => storage.SetValue(variable.GetName(), variable.floatValue));

            // Add all the bool variables to the dictionaries
            boolVariables.ForEach(variable => storage.SetValue(variable.GetName(), variable.boolValue));
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

        public void GetStringVariables()
        {
            // Get all the string variables from the storage and create them in the variable data
            Dictionary<string, string> stringVariables = storage.GetAllVariables().Item2;

            // Iterate through each string variable and create it in the variable data
            foreach (KeyValuePair<string, string> variable in stringVariables) CreateVariable(variable.Key, variable.Value);
        }

        public void GetFloatVariables()
        {
            // Get all the float variables from the storage and create them in the variable data
            Dictionary<string, float> floatVariables = storage.GetAllVariables().Item1;

            // Iterate through each float variable and create it in the variable data
            foreach (KeyValuePair<string, float> variable in floatVariables) CreateVariable(variable.Key, variable.Value);
        }

        public void GetBoolVariables()
        {
            // Get all the bool variables from the storage and create them in the variable data
            Dictionary<string, bool> boolVariables = storage.GetAllVariables().Item3;

            // Iterate through each bool variable and create it in the variable data
            foreach (KeyValuePair<string, bool> variable in boolVariables) CreateVariable(variable.Key, variable.Value);
        }

        #endregion

        #region Create Variable Methods

        public void CreateVariable(string name, string value) => stringVariables.Add(new Variable(name, value));

        public void CreateVariable(string name, float value) => floatVariables.Add(new Variable(name, value));

        public void CreateVariable(string name, bool value) => boolVariables.Add(new Variable(name, value));

        #endregion

        #region Set Variable Methods

        public void SetValue(string variableName, string value)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            storage.SetValue(variableName, value);

            // Find the variable in the variables list and update the value
            stringVariables.Find(variable => variable.GetName() == variableName)?.SetValue(value);
        }

        public void SetValue(string variableName, float value)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            storage.SetValue(variableName, value);

            // Find the variable in the variables list and update the value
            floatVariables.Find(variable => variable.GetName() == variableName)?.SetValue(value);
        }

        public void SetValue(string variableName, bool value)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            storage.SetValue(variableName, value);

            // Find the variable in the variables list and update the value
            boolVariables.Find(variable => variable.GetName() == variableName)?.SetValue(value);
        }

        public void SetAllVariables(Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            // Set all string variables in the storage
            storage.SetAllVariables(floatVariables, stringVariables, boolVariables);

            // Get all variables from the storage
            GetAllVariables();
        }

        [ContextMenu("Clear All Variables")]
        public void ClearAllVariables()
        {
            // Clear the variable storage
            storage?.Clear();

            // Clear all variable lists
            Clear();
        }

        #endregion

        #region Get Variable Methods

        public string GetVariable(string variableName, out string value)
        {
            // Initialize the out value
            value = string.Empty;

            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the storage is null, check the string variables list
            if (storage == null)
            {
                // Check the float variables list for the variable name first
                foreach (Variable variable in stringVariables)
                {
                    // If the variable name matches, get the string value
                    if (variable.GetName() == variableName)
                    {
                        // Get the string value from the variable
                        value = variable.stringValue;

                        // Return the string value
                        return value;
                    }
                }
            }
            else
            {
                // If the variable does not exist in the variable storage, log an error and return an empty string
                if (!storage.Contains(variableName))
                {
                    Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                    value = string.Empty;
                    return value;
                }

                // Try to get the value of the variable
                storage.TryGetValue(variableName, out value);
            }

            // Return the value of the variable
            return value;
        }

        public float GetVariable(string variableName, out float value)
        {
            // Initialize the out value
            value = 0f;

            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the storage is null, check the float variables list
            if (storage == null)
            {
                // Check the float variables list for the variable name first
                foreach (Variable variable in floatVariables)
                {
                    // If the variable name matches, get the float value
                    if (variable.GetName() == variableName)
                    {
                        // Get the float value from the variable
                        value = variable.floatValue;
                        // Return the float value
                        return value;
                    }
                }
            }
            else
            {
                // If the variable does not exist in the variable storage, log an error and return 0
                if (!storage.Contains(variableName))
                {
                    Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                    value = 0f;
                    return value;
                }

                // Try to get the value of the variable
                storage.TryGetValue(variableName, out value);
            }

            // Return the value of the variable
            return value;
        }

        public bool GetVariable(string variableName, out bool value)
        {
            // Initialize the out value
            value = false;

            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the storage is null, check the bool variables list
            if (storage == null)
            {
                // Check the bool variables list for the variable name first
                foreach (Variable variable in boolVariables)
                {
                    // If the variable name matches, get the bool value
                    if (variable.GetName().Equals(variableName))
                    {
                        // Get the bool value from the variable
                        value = variable.boolValue;

                        // Return the bool value
                        return value;
                    }
                }
            }
            else             
            {
                // If the variable does not exist in the variable storage, log an error and return false
                if (!storage.Contains(variableName))
                {
                    Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                    value = false;
                    return value;
                }

                // Try to get the value of the variable
                storage.TryGetValue(variableName, out value);
            }

            // Return the value of the variable
            return value;
        }

        public Type GetVariableType(string variableName)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return null
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                return null;
            }

            // Try to get the type of the variable
            if (storage.TryGetValue(variableName, out string stringValue))
            {
                return typeof(string);
            }
            else if (storage.TryGetValue(variableName, out float floatValue))
            {
                return typeof(float);
            }
            else if (storage.TryGetValue(variableName, out bool boolValue))
            {
                return typeof(bool);
            }

            // If the variable type is not found, log an error
            Debug.LogError("Variable type for: " + variableName + " not found in the variable storage.");

            // Return null if the variable type is not found
            return null;
        }

        public (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables() => storage.GetAllVariables();

        #endregion

        #region List Management

        public void Remove(Variable variable)
        {
            // Check if the variable is in the appropriate list and remove it
            if (variable.IsString && stringVariables.Contains(variable))
            {
                stringVariables.Remove(variable);
            }
            else if (variable.IsFloat && floatVariables.Contains(variable))
            {
                floatVariables.Remove(variable);
            }
            else if (variable.IsBool && boolVariables.Contains(variable))
            {
                boolVariables.Remove(variable);
            }
        }

        public bool HasAllVariables()
        {
            // Initialize the hasAllVariables variable to true
            bool hasAllVariables = true;

            // Check if the string variables are in the storage
            foreach (Variable variable in stringVariables)
            {
                if (!storage.Contains(variable.Name))
                {
                    hasAllVariables = false;
                    return hasAllVariables;
                }
            }

            // Check if the float variables are in the storage
            foreach (Variable variable in floatVariables)
            {
                if (!storage.Contains(variable.Name))
                {
                    hasAllVariables = false;
                    return hasAllVariables;
                }
            }

            // Check if the bool variables are in the storage
            foreach (Variable variable in boolVariables)
            {
                if (!storage.Contains(variable.Name))
                {
                    hasAllVariables = false;
                    return hasAllVariables;
                }
            }

            // Return the hasAllVariables variable
            return hasAllVariables;
        }

        public bool Equal()
        {
            // Initialize the equal variable to true
            bool equal = true;

            // Check if the string variables are equal to the storage
            foreach (Variable variable in stringVariables)
            {
                if (!storage.Contains(variable.Name))
                {
                    equal = false;
                    return equal;
                }

                if (storage.TryGetValue(variable.Name, out string value))
                {
                    if (variable.stringValue != value)
                    {
                        equal = false;
                        return equal;
                    }
                }
            }

            // Check if the float variables are equal to the storage
            foreach (Variable variable in floatVariables)
            {
                if (!storage.Contains(variable.Name))
                {
                    equal = false;
                    return equal;
                }

                if (storage.TryGetValue(variable.Name, out float value))
                {
                    if (variable.floatValue != value)
                    {
                        equal = false;
                        return equal;
                    }
                }
            }

            // Check if the bool variables are equal to the storage
            foreach (Variable variable in boolVariables)
            {
                if (!storage.Contains(variable.Name))
                {
                    equal = false;
                    return equal;
                }

                if (storage.TryGetValue(variable.Name, out bool value))
                {
                    if (variable.boolValue != value)
                    {
                        equal = false;
                        return equal;
                    }
                }
            }

            // Return the equal variable
            return equal;
        }

        public void Clear()
        {
            // Clear all variable lists
            stringVariables.Clear();
            floatVariables.Clear();
            boolVariables.Clear();
        }

        #endregion
    }
}
