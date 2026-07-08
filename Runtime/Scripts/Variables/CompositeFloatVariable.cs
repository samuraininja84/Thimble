using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thimble
{
    public struct CompositeFloatVariable : IVariable<float>, IEquatable<float>, IEquatable<CompositeFloatVariable>
    {
        [SerializeField] private string name;
        [SerializeField] private float value;

        public string Name { readonly get => name; set => name = value; }

        public float Value { readonly get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from float to CompositeFloatVariable
        public static implicit operator float(CompositeFloatVariable variable) => variable.Value;

        // Equality operators for comparing CompositeFloatVariable with CompositeFloatVariable
        public static bool operator ==(CompositeFloatVariable left, CompositeFloatVariable right) => left.Equals(right);
        public static bool operator !=(CompositeFloatVariable left, CompositeFloatVariable right) => !left.Equals(right);

        // Equality operators for comparing CompositeFloatVariable with float
        public static bool operator ==(CompositeFloatVariable variable, float value) => variable.Equals(value);
        public static bool operator !=(CompositeFloatVariable variable, float value) => !variable.Equals(value);

        #endregion

        CompositeFloatVariable(string name, float value)
        {
            this.name = name;
            this.value = value;
        }

        CompositeFloatVariable(List<string> names, float value)
        {
            this.name = string.Join(", ", names);
            this.value = value;
        }

        public static CompositeFloatVariable Default => new(VariableHandler.MissingVariableName, 0f);

        public static CompositeFloatVariable Create(string name, float value) => new(name, value);

        public static CompositeFloatVariable Create(List<string> names, float value) => new(names, value);

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

        public void SetValue(float value)
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

        public float GetValue()
        {
            // Trim whitespace from the name string
            var temp = name.Trim();

            // Split the name string into a list of names using comma as the delimiter and trim whitespace from each name
            var names = new List<string>(temp.Split(',', StringSplitOptions.RemoveEmptyEntries));

            // Return the sum of all values in the values array. This effectively computes the total of all variable values.
            return Value = VariableData.Instance.GetValueSum(names);
        }

        public readonly bool Equals(IVariable<float> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && Value == other.Value;
        }

        public readonly bool Equals(CompositeFloatVariable other) => Equals((IVariable<float>)other);

        public readonly bool Equals(float other) => Value == other;

        public override readonly bool Equals(object obj)
        {
            if (obj is CompositeFloatVariable composite)
                return Equals(composite);
            if (obj is float f)
                return Equals(f);
            return false;
        }

        public override readonly int GetHashCode() => (Name, Value).GetHashCode();
    }
}