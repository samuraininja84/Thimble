using System;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public struct FloatVariable : IVariable<float>, IEquatable<float>, IEquatable<FloatVariable>
    {
        [SerializeField] private string name;
        [SerializeField] private float value;

        public string Name { readonly get => name; set => name = value; }

        public float Value { readonly get => value; set => this.value = value; }

        #region Operators

        // Implicit conversion from float to FloatVariable
        public static implicit operator float(FloatVariable variable) => variable.Value;

        // Equality operators for comparing FloatVariable with FloatVariable
        public static bool operator ==(FloatVariable left, FloatVariable right) => left.Equals(right);
        public static bool operator !=(FloatVariable left, FloatVariable right) => !left.Equals(right);

        // Equality operators for comparing FloatVariable with float
        public static bool operator ==(FloatVariable variable, float value) => variable.Equals(value);
        public static bool operator !=(FloatVariable variable, float value) => !variable.Equals(value);

        #endregion

        FloatVariable(string name, float value)
        {
            this.name = name;
            this.value = value;
        }

        public static FloatVariable Default => new(VariableHandler.MissingVariableName, 0f);

        public static FloatVariable Create(string name, float value) => new(name, value);

        public void SetName(string name) => Name = name;

        public void SetValue(float value)
        {
            // If the variable name is empty, return without setting the value to avoid creating an entry in VariableData with an empty name
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return;

            // Set the variable value in the VariableData using the variable's name
            VariableData.Instance.SetVariable(Name, value);

            // Update the value in case it was changed in the VariableData
            Value = value;
        }

        public readonly string GetName() => Name.AppendYarnPrefix();

        public float GetValue()
        {
            // If the variable name is empty, return the current value without trying to get it from VariableData
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return Value;

            // Try to get the variable value from the VariableData using the variable's name
            VariableData.Instance.GetVariable(Name, out float value);

            // Update the value in case it was changed in the VariableData
            return Value = value;
        }

        public readonly bool Equals(IVariable<float> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && other.Value.Approximately(Value);
        }

        public readonly bool Equals(FloatVariable other) => Equals((IVariable<float>)other);

        public readonly bool Equals(float other) => other.Approximately(Value);

        public override readonly bool Equals(object obj)
        {
            if (obj is FloatVariable variable)
                return Equals(variable);
            if (obj is float f)
                return Equals(f);
            return false;
        }

        public override readonly int GetHashCode() => (Name, Value).GetHashCode();
    }
}