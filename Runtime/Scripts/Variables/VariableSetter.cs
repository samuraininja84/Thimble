using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableSetter
    {
        public static void SetFloatVariable(InMemoryVariableStorage variables, string variableName, float value)
        {
            // If the variable does not exist in the variable storage, log an error and return
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            variables.SetValue(variableName, value);
        }

        public static void SetIntVariable(InMemoryVariableStorage variables, string variableName, int value)
        {
            // If the variable does not exist in the variable storage, log an error and return
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            variables.SetValue(variableName, value);
        }

        public static void SetBoolVariable(InMemoryVariableStorage variables, string variableName, bool value)
        {
            // If the variable does not exist in the variable storage, log an error and return
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            variables.SetValue(variableName, value);
        }

        public static void SetStringVariable(InMemoryVariableStorage variables, string variableName, string value)
        {
            // If the variable does not exist in the variable storage, log an error and return
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return;
            }

            // Set the value of the variable
            variables.SetValue(variableName, value);
        }

        public static void SetAllVariables(InMemoryVariableStorage variables, Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            variables.SetAllVariables(floatVariables, stringVariables, boolVariables);
        }

        public static void Clear(InMemoryVariableStorage variables)
        {
            variables.Clear();
        }
    }
}