using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableRetriever
    {
        public static float GetFloatVariable(InMemoryVariableStorage variables, string variableName, float returnVal = 0f)
        {
            // If the variable does not exist in the variable storage, log an error and return 0f
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return 0f;
            }

            // Try to get the value of the variable
            variables.TryGetValue(variableName, out returnVal);

            // Return the value of the variable
            return returnVal;
        }

        public static int GetIntVariable(InMemoryVariableStorage variables, string variableName, int returnVal = 0)
        {
            // If the variable does not exist in the variable storage, log an error and return 0
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return 0;
            }

            // Try to get the value of the variable
            variables.TryGetValue(variableName, out returnVal);

            // Return the value of the variable
            return returnVal;
        }

        public static bool GetBoolVariable(InMemoryVariableStorage variables, string variableName, bool returnVal = false)
        {
            // If the variable does not exist in the variable storage, log an error and return false
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return false;
            }

            // Try to get the value of the variable
            variables.TryGetValue(variableName, out returnVal);

            // Return the value of the variable
            return returnVal;
        }

        public static string GetStringVariable(InMemoryVariableStorage variables, string variableName, string returnVal = "")
        {
            // If the variable does not exist in the variable storage, log an error and return an empty string
            if (!variables.Contains(variableName))
            {
                Debug.LogError("Variable " + variableName + " does not exist in the variable storage.");
                return string.Empty;
            }

            // Try to get the value of the variable
            variables.TryGetValue(variableName, out returnVal);

            // Return the value of the variable
            return returnVal;
        }

        public static (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables(InMemoryVariableStorage variables)
        {
            return variables.GetAllVariables();
        }
    }
}