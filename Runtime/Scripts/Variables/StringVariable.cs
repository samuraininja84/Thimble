using System;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public struct StringVariable : IVariable<string>, IEquatable<string>, IEquatable<StringVariable>
    {
        [SerializeField] private string name;
        [SerializeField] private string value;

        public string Name { readonly get => name; set => name = value; }

        public string Value { readonly get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from string to StringVariable
        public static implicit operator string(StringVariable variable) => variable.Value;

        // Equality operators for comparing StringVariable with StringVariable
        public static bool operator ==(StringVariable left, StringVariable right) => left.Equals(right);
        public static bool operator !=(StringVariable left, StringVariable right) => !left.Equals(right);

        // Equality operators for comparing StringVariable with string
        public static bool operator ==(StringVariable variable, string value) => variable.Equals(value);
        public static bool operator !=(StringVariable variable, string value) => !variable.Equals(value);

        #endregion

        public StringVariable(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public static StringVariable Default => new(VariableHandler.MissingVariableName, string.Empty);

        public static StringVariable Create(string name, string value) => new(name, value);

        public void SetName(string name) => Name = name;

        public void SetValue(string value)
        {
            // If the variable name is empty, return without setting the value to avoid creating an entry in VariableData with an empty name
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return;

            // Set the variable value in the VariableData using the variable's name
            VariableData.Instance.SetVariable(Name, value);

            // Update the value in case it was changed in the VariableData
            Value = value;
        }

        public readonly string GetName() => Name.AppendYarnPrefix();

        public string GetValue()
        {
            // If the variable name is empty, return the current value without trying to get it from VariableData
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return Value;

            // Try to get the variable value from the VariableData using the variable's name
            VariableData.Instance.GetVariable(Name, out string value);

            // Update the value in case it was changed in the VariableData
            return Value = value;
        }

        public readonly bool Equals(IVariable<string> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // Compare values using case-insensitive comparison
            bool valueEquals = string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && valueEquals;
        }

        public readonly bool Equals(StringVariable other) => Equals((IVariable<string>)other);

        public readonly bool Equals(string other) => string.Equals(Value, other, StringComparison.OrdinalIgnoreCase);

        public override readonly bool Equals(object obj)
        {
            if (obj is StringVariable variable) return Equals(variable);
            if (obj is string str) return Equals(str);
            return false;
        }

        public override readonly int GetHashCode() => (Name, Value).GetHashCode();
    }
}