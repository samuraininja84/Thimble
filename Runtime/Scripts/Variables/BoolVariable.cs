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

        public static BoolVariable Default => new BoolVariable(VariableExtensions.MissingVariableName, false);

        public static BoolVariable Create(string name, bool value) => new BoolVariable(name, value);

        public void SetName(string name) => Name = name;

        public void SetValue(bool value) => Value = value;

        public void SetValue(IVariable<bool> variable) => Value = variable.Value;

        public string GetName() => VariableExtensions.Prefix + Name;

        public bool GetValue() => Value;

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