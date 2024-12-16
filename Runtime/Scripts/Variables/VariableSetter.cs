using System.Collections.Generic;
using Yarn.Unity;

namespace Thimble
{
    public static class VariableSetter
    {
        public static void SetVariable(InMemoryVariableStorage storage, VariableData variableData, string variableName, string value)
        {
            variableData.SetValue(storage, variableName, value);
        }

        public static void SetVariable(InMemoryVariableStorage storage, VariableData variableData, string variableName, float value)
        {
            variableData.SetValue(storage, variableName, value);
        }

        public static void SetVariable(InMemoryVariableStorage storage, VariableData variableData, string variableName, bool value)
        {
            variableData.SetValue(storage, variableName, value);
        }

        public static void SetAllVariables(InMemoryVariableStorage storage, VariableData variableData, Dictionary<string, float> floatVariables, Dictionary<string, string> stringVariables, Dictionary<string, bool> boolVariables)
        {
            variableData.SetAllVariables(storage, floatVariables, stringVariables, boolVariables);
        }

        public static void Clear(InMemoryVariableStorage storage)
        {
            storage.Clear();
        }
    }
}