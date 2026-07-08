using System;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public struct BoolVariable : IVariable<bool>, IEquatable<bool>, IEquatable<BoolVariable>
    {
        [SerializeField] private string name;
        [SerializeField] private bool value;

        public string Name { readonly get => name; set => name = value; }

        public bool Value { readonly get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from bool to BoolVariable
        public static implicit operator bool(BoolVariable variable) => variable.Value;

        // Equality operators for comparing BoolVariable with BoolVariable
        public static bool operator ==(BoolVariable left, BoolVariable right) => left.Equals(right);
        public static bool operator !=(BoolVariable left, BoolVariable right) => !left.Equals(right);

        // Equality operators for comparing BoolVariable with bool
        public static bool operator ==(BoolVariable variable, bool value) => variable.Equals(value);
        public static bool operator !=(BoolVariable variable, bool value) => !variable.Equals(value);

        #endregion

        BoolVariable(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }

        public static BoolVariable Default => new(VariableHandler.MissingVariableName, false);

        public static BoolVariable Create(string name, bool value) => new(name, value);

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

        public readonly string GetName() => Name.AppendYarnPrefix();

        public bool GetValue()
        {
            // If the variable name is empty, return the current value without trying to get it from VariableData
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return Value;

            // Try to get the variable value from the VariableData using the variable's name
            VariableData.Instance.GetVariable(Name, out bool value);

            // Update the value in case it was changed in the VariableData
            return Value = value;
        }

        public readonly bool Equals(IVariable<bool> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && Value == other.Value;
        }

        public readonly bool Equals(BoolVariable other) => Equals((IVariable<bool>)other);

        public readonly bool Equals(bool other) => Value == other;

        public override readonly bool Equals(object obj)
        {
            if (obj is BoolVariable variable)
                return Equals(variable);
            if (obj is bool b)
                return Equals(b);
            return false;
        }

        public override readonly int GetHashCode() => (Name, Value).GetHashCode();
    }
}