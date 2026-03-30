using UnityEngine;

namespace Thimble
{
    public class TestVariables : MonoBehaviour
    {
        [Header("Old Variables")]
        public Variable floatVariable = new Variable("Float Variable", 0f);
        public Variable stringVariable = new Variable("String Variable", "Hello World");
        public Variable boolVariable = new Variable("Bool Variable", true);

        [Header("New Variables")]
        public FloatVariable newFloatVariable = FloatVariable.Default;
        public StringVariable newStringVariable = StringVariable.Default;
        public BoolVariable newBoolVariable = BoolVariable.Default;

        [Header("Other")]
        [YarnVariableDropdown("Assets/Resources/Dialogue/Yarn")] public string variableName;
    }
}
