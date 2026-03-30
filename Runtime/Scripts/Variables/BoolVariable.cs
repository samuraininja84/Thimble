using System;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public struct BoolVariable : IVariable<bool>
    {
        [SerializeField] private string name;
        [SerializeField] private bool value;

        public string Name { get => name; set => name = value; }

        public bool Value { get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from bool to BoolVariable and vice versa
        public static implicit operator BoolVariable(bool value) => new BoolVariable { Name = value.ToString(), Value = value };
        public static implicit operator bool(BoolVariable variable) => variable.Value;

        // Equality operators for comparing BoolVariable with BoolVariable
        public static bool operator ==(BoolVariable left, BoolVariable right) => left.Equals(right);
        public static bool operator !=(BoolVariable left, BoolVariable right) => !left.Equals(right);

        // Equality operators for comparing BoolVariable with bool
        public static bool operator ==(BoolVariable variable, bool value) => variable.Equals(value);
        public static bool operator !=(BoolVariable variable, bool value) => !variable.Equals(value);

        #endregion

        public BoolVariable(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }

        public static BoolVariable Default => new BoolVariable(VariableHandler.MissingVariableName, false);

        public static BoolVariable Create(string name, bool value) => new BoolVariable(name, value);

        public void SetName(string name) => Name = name;

        public void SetValue(bool value)
        {
            // If the variable name is empty, return without setting the value to avoid creating an entry in VariableData with an empty name
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return;

            // Set the variable value in the VariableData using the variable's name
            VariableData.Instance.SetVariable(Name, value);

            // Update the value in case it was changed in the VariableData
            Value = value;
        }

        public void SetValue(IVariable<bool> variable) => Value = variable.Value;

        public string GetName() => VariableHandler.Prefix + Name;

        public bool GetValue()
        {
            // If the variable name is empty, return the current value without trying to get it from VariableData
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return Value;

            // Try to get the variable value from the VariableData using the variable's name
            VariableData.Instance.GetVariable(Name, out bool value);

            // Update the value in case it was changed in the VariableData
            return Value = value;
        }

        public bool Equals(IVariable<bool> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && Value == other.Value;
        }

        public bool Equals(BoolVariable other) => Equals((IVariable<bool>)other);

        public bool Equals(bool other) => Value == other;

        public override bool Equals(object obj)
        {
            if (obj is BoolVariable variable)
                return Equals(variable);
            if (obj is bool b)
                return Equals(b);
            return false;
        }

        public override int GetHashCode() => (Name, Value).GetHashCode();
    }
}