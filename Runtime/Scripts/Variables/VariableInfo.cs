namespace Thimble
{
    public struct VariableInfo
    {
        public string name;
        private bool smartVariable;

        public VariableInfo(string name, bool smartVariable = false)
        {
            this.name = name;
            this.smartVariable = smartVariable;
        }

        public bool IsSmartVariable() => smartVariable;

        // Implicit conversion to string for easy display in the inspector
        public static implicit operator string(VariableInfo variableInfo) => variableInfo.name;
    }
}
