using System.Collections.Generic;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableHandler
    {
        #region Variable Setters

        public static void SetVariable(VariableData variableData, string variableName, string value) => variableData.SetValue(variableName, value);

        public static void SetVariable(VariableData variableData, string variableName, float value) => variableData.SetValue(variableName, value);

        public static void SetVariable(VariableData variableData, string variableName, bool value) => variableData.SetValue(variableName, value);

        public static void SetAllVariables(VariableData variableData, Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            variableData.SetAllVariables(floatVariables, stringVariables, boolVariables);
        }

        public static void Clear(VariableStorageBehaviour storage) => storage.Clear();

        #endregion

        #region Variable Getters

        public static System.Type GetVariableType(VariableData variableData, string variableName) => variableData.GetVariableType(variableName);

        public static string GetVariable(VariableData variableData, string variableName, string value = "") => variableData.GetVariable(variableName, value);

        public static float GetVariable(VariableData variableData, string variableName, float value = 0f) => variableData.GetVariable(variableName, value);

        public static bool GetVariable(VariableData variableData, string variableName, bool value = false) => variableData.GetVariable(variableName, value);

        public static Dictionary<string, string> GetStringVariables(VariableStorageBehaviour storage) => storage.GetAllVariables().Item2;

        public static Dictionary<string, float> GetFloatVariables(VariableStorageBehaviour storage) => storage.GetAllVariables().Item1;

        public static Dictionary<string, bool> GetBoolVariables(VariableStorageBehaviour storage) => storage.GetAllVariables().Item3;

        public static (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables(VariableStorageBehaviour storage) => storage.GetAllVariables();

        #endregion
    }
}