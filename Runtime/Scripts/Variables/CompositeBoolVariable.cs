using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thimble
{
    public struct CompositeBoolVariable : IVariable<bool>, IEquatable<bool>, IEquatable<CompositeBoolVariable>
    {
        [SerializeField] private List<string> names;
        [SerializeField] private bool value;

        public string Name { readonly get => string.Join(", ", names); set => names = new List<string> { value }; }

        public bool Value { readonly get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from bool to CompositeBoolVariable
        public static implicit operator bool(CompositeBoolVariable variable) => variable.Value;

        // Equality operators for comparing CompositeBoolVariable with CompositeBoolVariable
        public static bool operator ==(CompositeBoolVariable left, CompositeBoolVariable right) => left.Equals(right);
        public static bool operator !=(CompositeBoolVariable left, CompositeBoolVariable right) => !left.Equals(right);

        // Equality operators for comparing CompositeBoolVariable with bool
        public static bool operator ==(CompositeBoolVariable variable, bool value) => variable.Equals(value);
        public static bool operator !=(CompositeBoolVariable variable, bool value) => !variable.Equals(value);

        #endregion

        CompositeBoolVariable(List<string> names, bool value)
        {
            this.names = names;
            this.value = value;
        }

        public static CompositeBoolVariable Create(List<string> names, bool value) => new(names, value);

        public void SetName(string name) => names = new List<string> { name };

        public void SetValue(bool value)
        {
            // For each name in the list, set the variable value in the VariableData using the variable's name
            foreach (var name in names)
            {
                // If the variable name is empty or matches the missing variable name, skip setting the value to avoid creating an entry in VariableData with an empty name
                if (string.IsNullOrEmpty(name) || string.Equals(name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) continue;

                // Set the variable value in the VariableData using the variable's name
                VariableData.Instance.SetVariable(name, value);
            }

            // Update the value in case it was changed in the VariableData
            Value = value;
        }

        public readonly string GetName() => Name.AppendYarnPrefix();

        public bool GetValue()
        {
            // Return false if the list of names is empty to avoid trying to get a value from VariableData with an empty name
            if (names.Count == 0) return Value = false;

            // For each name in the list, get the variable value from the VariableData using the variable's name
            foreach (var name in names)
            {
                // If the variable name is empty or matches the missing variable name, skip getting the value to avoid trying to get it from VariableData
                if (string.IsNullOrEmpty(name) || string.Equals(name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) continue;

                // Get the variable value from the VariableData using the variable's name
                VariableData.Instance.GetVariable(name, out bool value);

                // If the value is false, return false immediately to short-circuit the logical AND operation
                if (!value) return Value = false; 
            }

            // Return true if all values in the values array are true, otherwise return false. This effectively computes the logical AND of all variable values.
            return Value = true;
        }

        public readonly bool Equals(IVariable<bool> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && Value == other.Value;
        }

        public readonly bool Equals(CompositeBoolVariable other) => Equals((IVariable<bool>)other);

        public readonly bool Equals(bool other) => Value == other;

        public override readonly bool Equals(object obj)
        {
            if (obj is CompositeBoolVariable composite)
                return Equals(composite);
            if (obj is bool b)
                return Equals(b);
            return false;
        }

        public override readonly int GetHashCode() => (Name, Value).GetHashCode();
    }
}