using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public struct CompositeBoolVariable : IVariable<bool>, IEquatable<bool>, IEquatable<CompositeBoolVariable>
    {
        [SerializeField] private string name;
        [SerializeField] private bool value;

        public string Name { readonly get => name; set => name = value; }

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

        CompositeBoolVariable(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }

        CompositeBoolVariable(List<string> names, bool value)
        {
            this.name = string.Join(", ", names);
            this.value = value;
        }

        public static CompositeBoolVariable Default => new(VariableHandler.MissingVariableName, false);

        public static CompositeBoolVariable Create(string name, bool value) => new(name, value);

        public static CompositeBoolVariable Create(List<string> names, bool value) => new(names, value);

        public void SetName(string name)
        {
            // Trim whitespace from the name string
            var temp = name.Trim();

            // Split the name string into a list of names using comma as the delimiter and trim whitespace from each name
            var names = new List<string>(temp.Split(",", StringSplitOptions.RemoveEmptyEntries));

            // For each name in the list, append the variable name in the VariableData using the variable's name
            foreach (var n in names) n.AppendYarnPrefix();

            // Update the name
            Name = string.Join(", ", names);
        }

        public void SetValue(bool value)
        {
            // Trim whitespace from the name string
            var temp = name.Trim();

            // Split the name string into a list of names using comma as the delimiter and trim whitespace from each name
            var names = new List<string>(temp.Split(',', StringSplitOptions.RemoveEmptyEntries));

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
            // Trim whitespace from the name string
            var temp = name.Trim();

            // Split the name string into a list of names using comma as the delimiter and trim whitespace from each name
            var names = new List<string>(temp.Split(',', StringSplitOptions.RemoveEmptyEntries));

            // Return true if all values in the values array are true, otherwise return false. This effectively computes the logical AND of all variable values.
            return Value = VariableData.Instance.GetConcatValue(names);
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