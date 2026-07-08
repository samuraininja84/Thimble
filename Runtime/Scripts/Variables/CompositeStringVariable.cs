using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thimble
{
    public struct CompositeStringVariable : IVariable<string>, IEquatable<string>, IEquatable<CompositeStringVariable>
    {
        [SerializeField] private string name;
        [SerializeField] private string value;

        public string Name { readonly get => name; set => name = value; }

        public string Value { readonly get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from string to CompositeStringVariable
        public static implicit operator string(CompositeStringVariable variable) => variable.Value;

        // Equality operators for comparing CompositeStringVariable with CompositeStringVariable
        public static bool operator ==(CompositeStringVariable left, CompositeStringVariable right) => left.Equals(right);
        public static bool operator !=(CompositeStringVariable left, CompositeStringVariable right) => !left.Equals(right);

        // Equality operators for comparing CompositeStringVariable with string
        public static bool operator ==(CompositeStringVariable variable, string value) => variable.Equals(value);
        public static bool operator !=(CompositeStringVariable variable, string value) => !variable.Equals(value);

        #endregion

        CompositeStringVariable(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        CompositeStringVariable(List<string> names, string value)
        {
            this.name = string.Join(", ", names);
            this.value = value;
        }

        public static CompositeStringVariable Default => new(VariableHandler.MissingVariableName, string.Empty);

        public static CompositeStringVariable Create(string name, string value) => new(name, value);

        public static CompositeStringVariable Create(List<string> names, string value) => new(names, value);

        public void SetName(string name) => this.name = name;

        public void SetValue(string value)
        {
            // Split the name string into a list of names using comma as the delimiter and trim whitespace from each name
            var names = new List<string>(name.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

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

        public string GetValue()
        {
            // Split the name string into a list of names using comma as the delimiter and trim whitespace from each name
            var names = new List<string>(name.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            // Return the concatenation of all values in the values array. This effectively computes the combined string of all variable values.
            return Value = VariableData.Instance.GetAppendedString(names);
        }

        public readonly bool Equals(IVariable<string> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && Value == other.Value;
        }

        public readonly bool Equals(CompositeStringVariable other) => Equals((IVariable<string>)other);

        public readonly bool Equals(string other) => Value == other;

        public override readonly bool Equals(object obj)
        {
            if (obj is CompositeStringVariable composite)
                return Equals(composite);
            if (obj is string s)
                return Equals(s);
            return false;
        }

        public override readonly int GetHashCode() => (Name, Value).GetHashCode();
    }
}