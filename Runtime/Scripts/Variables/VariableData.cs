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
        public InMemoryVariableStorage variableStorage;

        [Header("String Variables")]
        public List<Variable> stringVariables = new List<Variable>();

        [Header("Float Variables")]
        public List<Variable> floatVariables = new List<Variable>();

        [Header("Bool Variables")]
        public List<Variable> boolVariables = new List<Variable>();

        public delegate void OnCreation();
        public delegate void OnModified();
        public delegate void OnRemoved();

        public event OnCreation OnVariableCreated;
        public event OnModified OnVariableModified;
        public event OnRemoved OnVariableRemoved;

        #region Variable Management

        public void SortVariables()
        {
            // Sort the string, float, and bool variables by their names
            stringVariables.Sort((x, y) => x.Name.CompareTo(y.Name));
            floatVariables.Sort((x, y) => x.Name.CompareTo(y.Name));
            boolVariables.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        public void UpdateVariables(InMemoryVariableStorage storage)
        {
            // Add all the string variables to the dictionaries
            foreach (Variable variable in stringVariables)
            {
                storage.SetValue(variable.Name, variable.StringValue);
            }

            // Add all the float variables to the dictionaries
            foreach (Variable variable in floatVariables)
            {
                storage.SetValue(variable.Name, variable.FloatValue);
            }

            // Add all the bool variables to the dictionaries
            foreach (Variable variable in boolVariables)
            {
                storage.SetValue(variable.Name, variable.BoolValue);
            }

            // Get all the variables from the storage
            GetAllVariables(storage);
        }

        public void GetVariables(InMemoryVariableStorage storage)
        {
            // Clear all variable lists before getting new variables
            Clear();

            // Get all variables types from the storage
            GetStringVariables(storage);
            GetFloatVariables(storage);
            GetBoolVariables(storage);

            // Sort the variables after getting them
            SortVariables();
        }

        public void GetStringVariables(InMemoryVariableStorage storage)
        {
            // Get all the string variables from the storage and create them in the variable data
            Dictionary<string, string> stringVariables = storage.GetAllVariables().Item2;
            foreach (KeyValuePair<string, string> variable in stringVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void GetFloatVariables(InMemoryVariableStorage storage)
        {
            // Get all the float variables from the storage and create them in the variable data
            Dictionary<string, float> floatVariables = storage.GetAllVariables().Item1;
            foreach (KeyValuePair<string, float> variable in floatVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void GetBoolVariables(InMemoryVariableStorage storage)
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

        public void SetValue(InMemoryVariableStorage storage, string variableName, string value)
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
            foreach (Variable variable in stringVariables)
            {
                if (variable.Name == variableName)
                {
                    variable.StringValue = value;
                }
            }

            // Invoke the OnVariableModified event to notify listeners that a variable has changed
            OnVariableModified?.Invoke();
        }

        public void SetValue(InMemoryVariableStorage storage, string variableName, float value)
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
            foreach (Variable variable in floatVariables)
            {
                if (variable.Name == variableName)
                {
                    variable.FloatValue = value;
                }
            }

            // Invoke the OnVariableModified event to notify listeners that a variable has changed
            OnVariableModified.Invoke();
        }

        public void SetValue(InMemoryVariableStorage storage, string variableName, bool value)
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
            foreach (Variable variable in boolVariables)
            {
                if (variable.Name == variableName)
                {
                    variable.BoolValue = value;
                }
            }

            // Invoke the OnVariableModified event to notify listeners that a variable has changed
            OnVariableModified.Invoke();
        }

        public void SetAllVariables(InMemoryVariableStorage storage, Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            // Set all string variables in the storage
            storage.SetAllVariables(floatVariables, stringVariables, boolVariables);

            // Get all variables from the storage
            GetAllVariables(storage);

            // Invoke the OnVariableModified event to notify listeners that a variable has changed
            OnVariableModified.Invoke();
        }

        public void ClearAllVariables(InMemoryVariableStorage storage)
        {
            // Clear the variable storage
            storage.Clear();

            // Clear all variable lists
            Clear();

            // Invoke the OnVariableModified event to notify listeners that a variable has changed
            OnVariableRemoved?.Invoke();
        }

        #endregion

        #region Get Variable Methods

        public string GetVariable(InMemoryVariableStorage storage, string variableName, string value = "")
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

        public float GetVariable(InMemoryVariableStorage storage, string variableName, float value = 0f)
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

        public bool GetVariable(InMemoryVariableStorage storage, string variableName, bool value = false)
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

        public (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables(InMemoryVariableStorage storage)
        {
            return storage.GetAllVariables();
        }

        #endregion

        #region List Management

        public void Add(Variable variable)
        {
            // Initialize the added variable to false
            bool added = false;

            // Check if the variable is already in the appropriate list and add it if not
            if (variable.UseString && !stringVariables.Contains(variable))
            {
                stringVariables.Add(variable);
                added = true;
            }
            else if (variable.UseFloat && !floatVariables.Contains(variable))
            {
                floatVariables.Add(variable);
                added = true;
            }
            else if (variable.UseBool && !boolVariables.Contains(variable))
            {
                boolVariables.Add(variable);
                added = true;
            }

            // Invoke the OnVariableModified event to notify listeners that a variable has changed
            if (added) OnVariableCreated?.Invoke();
        }

        public void Remove(Variable variable)
        {
            // Initialize the removed variable to false
            bool removed = false;

            // Check if the variable is in the appropriate list and remove it
            if (variable.UseString && stringVariables.Contains(variable))
            {
                stringVariables.Remove(variable);
                removed = true;
            }
            else if (variable.UseFloat && floatVariables.Contains(variable))
            {
                floatVariables.Remove(variable);
                removed = true;
            }
            else if (variable.UseBool && boolVariables.Contains(variable))
            {
                boolVariables.Remove(variable);
                removed = true;
            }

            // Invoke the OnVariableRemoved event to notify listeners that a variable has been removed
            if (removed) OnVariableRemoved?.Invoke();
        }

        public bool HasAllVariables(InMemoryVariableStorage storage)
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

        public bool Equal(InMemoryVariableStorage storage)
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
                    if (variable.StringValue != value)
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
                    if (variable.FloatValue != value)
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
                    if (variable.BoolValue != value)
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
