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

        #region Variable Management

        public void SortVariables()
        {
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
            Clear();
            GetStringVariables(storage);
            GetFloatVariables(storage);
            GetBoolVariables(storage);
            SortVariables();
        }

        public void GetStringVariables(InMemoryVariableStorage storage)
        {
            Dictionary<string, string> stringVariables = storage.GetAllVariables().Item2;
            foreach (KeyValuePair<string, string> variable in stringVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void GetFloatVariables(InMemoryVariableStorage storage)
        {
            Dictionary<string, float> floatVariables = storage.GetAllVariables().Item1;
            foreach (KeyValuePair<string, float> variable in floatVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void GetBoolVariables(InMemoryVariableStorage storage)
        {
            Dictionary<string, bool> boolVariables = storage.GetAllVariables().Item3;
            foreach (KeyValuePair<string, bool> variable in boolVariables)
            {
                CreateVariable(variable.Key, variable.Value);
            }
        }

        public void CreateVariable(string name, string value)
        {
            Variable variable = new Variable(name, value);
            Add(variable);
        }

        public void CreateVariable(string name, float value)
        {
            Variable variable = new Variable(name, value);
            Add(variable);
        }

        public void CreateVariable(string name, bool value)
        {
            Variable variable = new Variable(name, value);
            Add(variable);
        }

        #endregion

        #region Set Variable Methods

        public void SetValue(InMemoryVariableStorage storage, string variableName, string value)
        {
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
        }

        public void SetValue(InMemoryVariableStorage storage, string variableName, float value)
        {
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
        }

        public void SetValue(InMemoryVariableStorage storage, string variableName, bool value)
        {
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
        }

        public void SetAllVariables(InMemoryVariableStorage storage, Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            storage.SetAllVariables(floatVariables, stringVariables, boolVariables);
            GetAllVariables(storage);
        }

        public void ClearAllVariables(InMemoryVariableStorage storage)
        {
            storage.Clear();
            Clear();
        }

        #endregion

        #region Get Variable Methods

        public string GetVariable(InMemoryVariableStorage storage, string variableName, string value = "")
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

            // Return the value of the variable
            return value;
        }

        public float GetVariable(InMemoryVariableStorage storage, string variableName, float value = 0f)
        {
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
            if (variable.UseString && !stringVariables.Contains(variable))
            {
                stringVariables.Add(variable);
            }
            else if (variable.UseFloat && !floatVariables.Contains(variable))
            {
                floatVariables.Add(variable);
            }
            else if (variable.UseBool && !boolVariables.Contains(variable))
            {
                boolVariables.Add(variable);
            }
        }

        public void Remove(Variable variable)
        {
            if (variable.UseString && stringVariables.Contains(variable))
            {
                stringVariables.Remove(variable);
            }
            else if (variable.UseFloat && floatVariables.Contains(variable))
            {
                floatVariables.Remove(variable);
            }
            else if (variable.UseBool && boolVariables.Contains(variable))
            {
                boolVariables.Remove(variable);
            }
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
            stringVariables.Clear();
            floatVariables.Clear();
            boolVariables.Clear();
        }

        #endregion
    }
}
