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

        [Header("String Variables")]
        public List<Variable> stringVariables = new();

        [Header("Float Variables")]
        public List<Variable> floatVariables = new();

        [Header("Bool Variables")]
        public List<Variable> boolVariables = new();

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
                Debug.LogError("Yarn Project is not set. Please set the Yarn Project before initializing variables.");
                return;
            }

            // Clear all variable lists before initializing
            Clear();

            // Get the initial values from the Yarn Project
            var values = yarnProject.InitialValues;

            // Iterate through the variable data and populate the dictionaries based on the variable type
            foreach (var pair in values)
            {
                // Get the value from the pair
                var value = pair.Value;

                // Check the type of the value and add it to the appropriate dictionary
                switch (value)
                {
                    case string stringValue:
                        CreateVariable(pair.Key, stringValue);
                        break;
                    case float floatValue:
                        CreateVariable(pair.Key, floatValue);
                        break;
                    case bool boolValue:
                        CreateVariable(pair.Key, boolValue);
                        break;
                }
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
                Debug.LogError("Variable storage is not set. Please set the variable storage before updating variables.");
                return;
            }

            // Add all the string variables to the dictionaries
            foreach (Variable variable in stringVariables)
            {
                storage.SetValue(variable.GetName(), variable.stringValue);
            }

            // Add all the float variables to the dictionaries
            foreach (Variable variable in floatVariables)
            {
                storage.SetValue(variable.GetName(), variable.floatValue);
            }

            // Add all the bool variables to the dictionaries
            foreach (Variable variable in boolVariables)
            {
                storage.SetValue(variable.GetName(), variable.boolValue);
            }

            // Get all the variables from the storage
            GetVariables();
        }

        public void GetVariables()
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
            foreach (KeyValuePair<string, string> variable in stringVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void GetFloatVariables()
        {
            // Get all the float variables from the storage and create them in the variable data
            Dictionary<string, float> floatVariables = storage.GetAllVariables().Item1;
            foreach (KeyValuePair<string, float> variable in floatVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void GetBoolVariables()
        {
            // Get all the bool variables from the storage and create them in the variable data
            Dictionary<string, bool> boolVariables = storage.GetAllVariables().Item3;
            foreach (KeyValuePair<string, bool> variable in boolVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        #endregion

        #region Create Variable Methods

        public void CreateVariable(string name, string value) => Add(new Variable(name, value));

        public void CreateVariable(string name, float value) => Add(new Variable(name, value));

        public void CreateVariable(string name, bool value) => Add(new Variable(name, value));

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

        public string GetVariable(string variableName, string value = "")
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return an empty string
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                value = string.Empty;
                return value;
            }

            // Try to get the value of the variable
            storage.TryGetValue(variableName, out value);

            // Return the value of the variable
            return value;
        }

        public float GetVariable(string variableName, float value = 0f)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return 0f
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                value = 0f;
                return value;
            }

            // Try to get the value of the variable
            storage.TryGetValue(variableName, out value);

            // Return the value of the variable
            return value;
        }

        public bool GetVariable(string variableName, bool value = false)
        {
            // Check if the variable name starts with a "$", if not add it
            if (!variableName.StartsWith("$")) variableName = "$" + variableName;

            // If the variable does not exist in the variable storage, log an error and return false
            if (!storage.Contains(variableName))
            {
                Debug.LogError("Variable: " + variableName + " does not exist in the variable storage.");
                value = false;
                return value;
            }

            // Try to get the value of the variable
            storage.TryGetValue(variableName, out value);

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

        public void Add(Variable variable)
        {
            // Check if the variable is already in the appropriate list and add it if not
            if (variable.IsString && !stringVariables.Contains(variable))
            {
                stringVariables.Add(variable);
            }
            else if (variable.IsFloat && !floatVariables.Contains(variable))
            {
                floatVariables.Add(variable);
            }
            else if (variable.IsBool && !boolVariables.Contains(variable))
            {
                boolVariables.Add(variable);
            }
        }

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
