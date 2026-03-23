using System.Collections.Generic;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableHandler
    {
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

        #endregion
    }
}