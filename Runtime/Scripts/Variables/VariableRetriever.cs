using System.Collections.Generic;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableRetriever
    {
        public static string GetVariable(InMemoryVariableStorage storage, VariableData variableData, string variableName, string value = "")
        {
            variableData.GetVariable(storage, variableName, value);
            return value;
        }

        public static float GetVariable(InMemoryVariableStorage storage, VariableData variableData, string variableName, float value = 0f)
        {
            variableData.GetVariable(storage, variableName, value);
            return value;
        }

        public static bool GetVariable(InMemoryVariableStorage storage, VariableData variableData, string variableName, bool value = false)
        {
            variableData.GetVariable(storage, variableName, value);
            return value;
        }

        public static Dictionary<string, string> GetStringVariables(InMemoryVariableStorage storage)
        {
            return storage.GetAllVariables().Item2;
        }

        public static Dictionary<string, float> GetFloatVariables(InMemoryVariableStorage storage)
        {
            return storage.GetAllVariables().Item1;
        }

        public static Dictionary<string, bool> GetBoolVariables(InMemoryVariableStorage storage)
        {
            return storage.GetAllVariables().Item3;
        }

        public static (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables(InMemoryVariableStorage storage)
        {
            return storage.GetAllVariables();
        }
    }
}