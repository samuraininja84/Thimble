using System.Collections.Generic;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableHandler
    {
        /// <summary>
        /// The prefix used to identify variable names in the system.
        /// </summary>
        public const string Prefix = "$";

        /// <summary>
        /// The default name used for variables that do not have a specified name. This can be used as a placeholder or to indicate that a variable is unnamed.
        /// </summary>
        public const string MissingVariableName = "None";

        /// <summary>
        /// The tag used to denote internal variables created by Yarn Spinner.
        /// </summary>
        public const string InternalVariableDenotator = "Yarn.Internal";

        /// <summary>
        /// Determines whether two floating-point values are approximately equal within a specified tolerance.
        /// </summary>
        /// <param name="a">The first floating-point value to compare.</param>
        /// <param name="b">The second floating-point value to compare.</param>
        /// <param name="epsilon">The maximum allowable difference between the two values for them to be considered approximately equal. The default value is 0.0001.</param>
        /// <returns><see langword="true"/> if the absolute difference between <paramref name="a"/> and <paramref name="b"/> is less than or equal to <paramref name="epsilon"/>; otherwise, <see langword="false"/>.</returns>
        public static bool Approximately(this float a, float b, float epsilon = 1E-06f) => UnityEngine.Mathf.Abs(a - b) <= epsilon;

        #region Variable Setters

        public static void SetVariable(this VariableData variableData, string variableName, string value) => variableData.SetValue(variableName, value);

        public static void SetVariable(this VariableData variableData, string variableName, float value) => variableData.SetValue(variableName, value);

        public static void SetVariable(this VariableData variableData, string variableName, bool value) => variableData.SetValue(variableName, value);

        #endregion

        #region Variable Getters

        public static System.Type GetVariableType(this VariableData variableData, string variableName) => variableData.GetVariableType(variableName);

        public static string GetVariable(this VariableData variableData, string variableName, out string value) => variableData.GetVariable(variableName, out value);

        public static float GetVariable(this VariableData variableData, string variableName, out float value) => variableData.GetVariable(variableName, out value);

        public static bool GetVariable(this VariableData variableData, string variableName, out bool value) => variableData.GetVariable(variableName, out value);

        public static Dictionary<string, string> GetStringVariables(this VariableStorageBehaviour storage) => storage.GetAllVariables().Item2;

        public static Dictionary<string, float> GetFloatVariables(this VariableStorageBehaviour storage) => storage.GetAllVariables().Item1;

        public static Dictionary<string, bool> GetBoolVariables(this VariableStorageBehaviour storage) => storage.GetAllVariables().Item3;

        public static Dictionary<string, string> GetStringVariables(this YarnProject project)
        {
            // Create a new dictionary to hold the string variables
            var variables = new Dictionary<string, string>();

            // Add the initial variables from the project to the dictionary
            foreach (var variable in project.InitialValues)
            {
                // Only add string variables to the dictionary
                if (variable.Value is string stringValue) variables.Add(variable.Key, stringValue);
            }

            // Add the default variables to the dictionary
            return variables;
        }

        public static Dictionary<string, float> GetFloatVariables(this YarnProject project)
        {
            // Create a new dictionary to hold the float variables
            var variables = new Dictionary<string, float>();

            // Add the initial variables from the project to the dictionary
            foreach (var variable in project.InitialValues)
            {
                // Only add float variables to the dictionary
                if (variable.Value is float floatValue) variables.Add(variable.Key, floatValue);
            }

            // Add the default variables to the dictionary
            return variables;
        }

        public static Dictionary<string, bool> GetBoolVariables(this YarnProject project)
        {
            // Create a new dictionary to hold the bool variables
            var variables = new Dictionary<string, bool>();

            // Add the initial variables from the project to the dictionary
            foreach (var variable in project.InitialValues)
            {
                // Only add bool variables to the dictionary
                if (variable.Value is bool boolValue) variables.Add(variable.Key, boolValue);
            }

            // Add the default variables to the dictionary
            return variables;
        }

        #endregion
    }
}